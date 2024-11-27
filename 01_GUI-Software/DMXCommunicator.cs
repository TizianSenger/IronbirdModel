using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace SerialCommunicator.DMX
{
    public class DMXCommunicator
    {
        private byte[] buffer = new byte[513];
        private bool isActive = false;
        private Task senderTask;
        private SerialPort serialPort;

        private const int DMX512_BAUD_RATE = 250000;

        public DMXCommunicator(string portName) : this(new SerialPort(portName)) { }

        public DMXCommunicator(SerialPort port)
        {
            buffer[0] = 0;
            serialPort = ConfigureSerialPort(port);
        }

        private static SerialPort ConfigureSerialPort(SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                    port.Close();

                port.BaudRate = DMX512_BAUD_RATE;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.Parity = Parity.None;
                port.StopBits = StopBits.Two;

                port.Open();
                port.Close();

                return port;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool IsActive
        {
            get
            {
                lock (this)
                {
                    return isActive;
                }
            }
        }

        public byte GetByte(int index)
        {
            if (index < 0 || index > 511)
                throw new IndexOutOfRangeException("Index is not between 0 and 511");

            lock (this)
            {
                return buffer[index + 1];
            }
        }

        public byte[] GetBytes()
        {
            byte[] newBuffer = new byte[512];
            lock (this)
            {
                Array.Copy(buffer, 1, newBuffer, 0, 512);
                return newBuffer;
            }
        }

        public static List<string> GetValidSerialPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            List<string> portNames = new List<string>();
            foreach (string port in ports)
            {
                try
                {
                    ConfigureSerialPort(new SerialPort(port));
                    portNames.Add(port);
                }
                catch (Exception) { }
            }
            return portNames;
        }

        public async Task SendBytesAsync()
        {
            while (isActive)
            {
                serialPort.BreakState = true;
                await Task.Delay(1);
                serialPort.BreakState = false;
                serialPort.Write(buffer, 0, buffer.Length);
            }
        }

        public void SetByte(int index, byte value)
        {
            if (index < 0 || index > 511)
                throw new IndexOutOfRangeException("Index is not between 0 and 511");

            lock (this)
            {
                buffer[index + 1] = value;
            }
        }

        public void SetBytes(byte[] newBuffer)
        {
            if (newBuffer.Length != 512)
                throw new ArgumentException("This byte array does not contain 512 elements", "newBuffer");

            lock (this)
            {
                Array.Copy(newBuffer, 0, buffer, 1, 512);
            }
        }

        public void Start()
        {
            if (!this.IsActive)
            {
                lock (this)
                {
                    if (!this.IsActive)
                    {
                        if (serialPort != null && !serialPort.IsOpen)
                            serialPort.Open();
                        this.isActive = true;
                        if (serialPort != null && serialPort.IsOpen)
                        {
                            senderTask = Task.Run(() => this.SendBytesAsync());
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            if (this.IsActive)
            {
                lock (this)
                {
                    if (this.IsActive)
                    {
                        this.isActive = false;
                        try
                        {
                            senderTask.Wait(1000);
                        }
                        catch (Exception)
                        {
                            // TODO: Better exception handling
                        }
                        if (serialPort != null && serialPort.IsOpen)
                            serialPort.Close();
                    }
                }
            }
        }
    }
}
