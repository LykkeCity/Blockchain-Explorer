using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TcpSockets
{
    public static class StreamExtentions
    {

        public static async Task ReadUntilAsync(this Stream stream, List<byte> list, byte symbol)
        {
            byte b;
            do
            {
                b = await stream.ReadByteFromSocket();
                list.Add(b);

            } while (b != symbol);

        }

        public static async Task<List<byte>> ReadUntilAsync(this Stream stream, byte symbol)
        {
            var result = new List<byte>();
            await stream.ReadUntilAsync(result, symbol);
            return result;
        }

        public static async Task<byte[]> ReadFromSocket(this Stream stream, int size)
        {
            var result = new byte[size];
            var read = 0;

            while (read < size)
            {
                var readChunkSize = await stream.ReadAsync(result, read, result.Length - read);

                // Если вычитанный кусок = 0, значит был разрыв соединения
                if (readChunkSize == 0)
                    throw new Exception("Disconnected");

                read += readChunkSize;

            }

            return result;
        }

        public static async Task<byte[]> ReadAsMuchAsPossible(this Stream stream, int size)
        {
            var buffer = new byte[size];

            var readChunkSize = await stream.ReadAsync(buffer, 0, size);

                // Если вычитанный кусок = 0, значит был разрыв соединения
                if (readChunkSize == 0)
                    throw new Exception("Disconnected");

            if (size == readChunkSize)
                return buffer;

            var result = new byte[readChunkSize];
            Array.Copy(buffer, 0, result, 0, readChunkSize);
            return result;
        }

        public static async Task<int> ReadAsMuchAsPossible(this Stream stream, byte[] buffer)
        {

            var readChunkSize = await stream.ReadAsync(buffer, 0, buffer.Length);

            // Если вычитанный кусок = 0, значит был разрыв соединения
            if (readChunkSize == 0)
                throw new Exception("Disconnected");

            return buffer.Length;
        }

        public static async Task<byte> ReadByteFromSocket(this Stream stream)
        {
            var result = await ReadFromSocket(stream, 1);
            return result[0];
        }



        public static async Task<uint> ReadUintFromSocket(this Stream stream)
        {
            var bytes = await ReadFromSocket(stream, 4);
            return bytes.ParseuInt();
        }

        public static async Task<string> ReadUtf8String(this Stream stream, int length, Encoding encoding)
        {
            var bytes = await ReadFromSocket(stream, length);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Считать строку из сокета в Pascal формате. Первый байт - длина строки, остальное строка
        /// </summary>
        /// <param name="stream">Поток, из которого читаем строку</param>
        /// <param name="encoding">Формат кодирования. По умолчанию: UTF-8</param>
        /// <returns>Считанная строка</returns>
        public static async Task<string> ReadPascalString(this Stream stream, Encoding encoding = null)
        {
            var srvLen = await stream.ReadByteFromSocket();
            var result = srvLen == 0 ? string.Empty : await stream.ReadUtf8String(srvLen, encoding?? Encoding.UTF8);
            return result;
        }

        /// <summary>
        /// Считать строку из потока в формате: 4 байта - длина строки и сама строка
        /// </summary>
        /// <param name="stream">Поток, из которого читается строка</param>
        /// <param name="encoding">Формат кодирования. По умолчанию: UTF-8</param>
        /// <returns>Полученная строка</returns>
        public static async Task<string> ReadString(this Stream stream, Encoding encoding = null)
        {
            var stringLength = await stream.ReadUintFromSocket();

            var result = stringLength == 0 ? string.Empty : await stream.ReadUtf8String((int)stringLength, encoding ?? Encoding.UTF8);

            return result;
        }


        public static void WritePascalString(this Stream stream, string data, Encoding encoding = null)
        {
            if (data == null)
                data = string.Empty;

            if (encoding == null)
                encoding = Encoding.UTF8;

            var dataBin = encoding.GetBytes(data);

            var stringLength = (byte)dataBin.Length;
            stream.WriteByte(stringLength);
            
            if (stringLength > 0)
                stream.Write(dataBin, 0, dataBin.Length);

        }

        public static byte[] ToPascalStringArray(this string data)
        {
            var arrayToAdd = data.ToUtf8Bytes();
            var list = new List<byte>(arrayToAdd.Length+1) { (byte)arrayToAdd.Length };
            list.AddRange(arrayToAdd);
            return list.ToArray();
        }

        public static void WriteString(this Stream stream, string data, Encoding encoding = null)
        {

            if (data == null)
                data = string.Empty;

            if (encoding == null)
                encoding = Encoding.UTF8;

            var dataBin = encoding.GetBytes(data);

            var stringLength = dataBin.Length;
            var bytesToSend = BitConverter.GetBytes(stringLength);
            stream.Write(bytesToSend, 0, bytesToSend.Length);

            if (stringLength > 0)
                stream.Write(dataBin, 0, dataBin.Length);
        }

    }



    public class StreamTextReader
    {
        private readonly byte[] _separator;
        private readonly List<byte> _buffer = new List<byte>();

        public StreamTextReader(byte[] separator)
        {
            _separator = separator;
        }

        public void LoadData(byte[] data)
        {
            _buffer.AddRange(data);
        }

        public IEnumerable<string> ReadStream()
        {
            var i = _buffer.IndexOf(_separator);

            while (i != -1)
            {
                var str = Encoding.ASCII.GetString(_buffer.CutFrom(0, i).ToArray());
                yield return str;
                _buffer.RemoveRange(0, i + _separator.Length);
                i = _buffer.IndexOf(_separator);
            }

        }

    }

}
