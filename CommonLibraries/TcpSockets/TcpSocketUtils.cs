using System;

namespace TcpSockets
{
    public class SocketStatistic
    {

        public void Init()
        {
            LastConnectionTime = DateTime.UtcNow;
            LastRecieveTime = DateTime.UtcNow;
            LastSendTime = DateTime.UtcNow;
            LastDisconnectionTime = DateTime.UtcNow;
        }

        public DateTime LastConnectionTime { get; set; }
        public DateTime LastSendTime { get; set; }
        public DateTime LastRecieveTime { get; set; }
        public DateTime LastDisconnectionTime { get; set; }

        public long Sent { get; set; }

        public long Recieved { get; set; }


    }


}
