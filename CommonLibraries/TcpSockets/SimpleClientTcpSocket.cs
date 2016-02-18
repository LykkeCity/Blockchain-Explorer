using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpSockets
{

    public interface ISocketSerializer
    {
        void Init();
        Task Deserialize(IEnumerable<byte> data);
        byte[] CreatePingPacket();
    }

    internal class SimpleTcpSocketConnection
    {
        private readonly SimpleClientTcpSocket _socket;
        private readonly TcpClient _tcpClient;
        private readonly ISocketSerializer _socketSerializer;
        public int Id { get;}
        private readonly ISocketLog _log;
        public bool Disconnected { get; private set; }
        private readonly object _lockObject = new object();

        internal DateTime LastReadData = DateTime.UtcNow;




        public SimpleTcpSocketConnection(SimpleClientTcpSocket socket, TcpClient tcpClient, ISocketSerializer socketSerializer, int id, ISocketLog socketLog)
        {
            _socket = socket;
            _tcpClient = tcpClient;
            _socketSerializer = socketSerializer;
            Id = id;
            _log = socketLog;
        }

        private async Task PingProcess()
        {
            var pingPacket = _socketSerializer.CreatePingPacket();
            if (pingPacket != null)
                while (!Disconnected)
                {
                    // Если никакого входящего 
                    if ((DateTime.UtcNow - LastReadData).TotalSeconds > _socket.PingTimeOut)
                    {
                        await Send(pingPacket);

                        // Ожидаем то что Ping пакет уйдет и придет ответ
                        await Task.Delay(_socket .PacketDeliveryTimeOut* 1000);

                        var now = DateTime.UtcNow;
                        var seconds = (now - LastReadData).TotalSeconds;
                        // Если за это время ответ на Ping не пришел, считаем что что связи нет и ее можно рвать
                        if (seconds > _socket.PingTimeOut)
                        {
                            _log.Add($"{_socket.SocketName}: Ping detected invalid connection:{Id}. Disconnect");
                            Disconnect();
                            break;
                        }

                    }
                    await Task.Delay(_socket.PingTimeOut);
                }
        }


        private async Task ReadData()
        {
            var stream = _tcpClient.GetStream();
            var gotfirstPacket = false;

            while (!Disconnected)
            {
                var bytes = await stream.ReadAsMuchAsPossible(1024);
                LastReadData = DateTime.UtcNow;
                if (!gotfirstPacket)
                {
                   _log.Add($"{_socket.SocketName}: Got first packet Sizeed:{bytes?.Length}. Connection:{Id}");
                    gotfirstPacket = true;
                }

                await _socketSerializer.Deserialize(bytes);
            }

        }



        public async Task ReadThread()
        {
            try
            {
                await Task.WhenAll(
                    ReadData(),
                    PingProcess()
                    );

            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                lock (_lockObject)
                {
                    if (Disconnected)
                        return;

                    Disconnected = true;
                }

                //_tcpClient.Close();
            }
            catch (Exception exception)
            {
                _log.Add($"{_socket.SocketName}: Disconnect error. Id={Id}; Msg: {exception.Message}");
            }

        }

        public Task Send(byte[] data)
        {
            if (Disconnected)
                return Task.FromResult(0);

            try
            {
                var stream = _tcpClient.GetStream();
                return stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception)
            {
                Disconnect();
            }

            return Task.FromResult(0);
        }


    }


    public class SimpleClientTcpSocket
    {
        internal readonly string SocketName;
        private readonly IPEndPoint _ipEndPoint;
        private readonly int _reconnectTimeOut;
        private readonly ISocketSerializer _serializer;
        private readonly ISocketLog _socketLog;
        internal readonly int PingTimeOut;
        internal readonly int PacketDeliveryTimeOut;

        private bool _working;

        private int _socketId;


        public SimpleClientTcpSocket(string socketName, IPEndPoint ipEndPoint, int reconnectTimeOut, ISocketSerializer serializer, ISocketLog socketLog = null, int pingTimeOut = 4, int packetDeliveryTimeOut=2)
        {
            SocketName = socketName;
            _ipEndPoint = ipEndPoint;
            _reconnectTimeOut = reconnectTimeOut;
            _serializer = serializer;
            _socketLog = socketLog;
            PingTimeOut = pingTimeOut;
            PacketDeliveryTimeOut = packetDeliveryTimeOut;
        }

        private async Task<SimpleTcpSocketConnection> Connect()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(_ipEndPoint.Address, _ipEndPoint.Port);

            return new SimpleTcpSocketConnection(this, tcpClient, _serializer, _socketId++, _socketLog);
        }


        public async void SocketThread()
        {
            while (_working)
            {
                try
                {
                    // Пытаемся создать соединение с сервером
                    var connection = await Connect();
                    _socketLog.Add($"Connected to server:{_ipEndPoint}. Id:{connection.Id}");
                    _serializer.Init();
                    // Запускаем процесс чтения данных. Он должен отвалиться сам когда потеряется связь
                    await connection.ReadThread();
                }

                catch (Exception ex)
                {
                    _socketLog.Add("Socket Thread Excepion: "+ex.Message);
                }

                await Task.Delay(_reconnectTimeOut);

            }

        }

        public void Start()
        {
            if (_working)
                throw new Exception("Client socket has already started");
            _working = true;
            SocketThread();
        }

    }

}
