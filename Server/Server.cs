using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientTCPServer.Server
{
    internal class Server
    {
        private TcpListener _server;
        private TcpClient _client;
        private Client _clientObj;
        private Thread _clientThread;
        internal delegate string Message();
        public int _port { get; private set; } // port server
        public string _ipAdress { get; private set; } // ipAdress server
        public List<string> _clientList;

        /// <summary>
        /// Запустить сервер
        /// </summary>
        public void Start()
        {
            Configuration();
            try
            {
                _clientList = new List<string>();
                _server = new TcpListener(IPAddress.Parse(_ipAdress), _port);
                _server.Start();
                Console.WriteLine("Сервер запущен......");
                Console.WriteLine("Сервер ожидает подключения......");
                Console.WriteLine("Возможные команды: ");
                foreach(var message in RoboText._roboText)
                {
                    Console.WriteLine($"{message.Key}");
                   
                }
                Console.WriteLine("------------------------------------------------------");
                while (true)
                {
                    Console.WriteLine("Ожидаю клиентов.......");
                    _client = _server.AcceptTcpClient();
                    _clientObj = new Client(_client, this);
                    _clientThread = new Thread(new ThreadStart(_clientObj.WorkWithClient));
                    _clientThread.Start();

                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: { ex.Message }");
            }
            finally
            {
                if (_server != null)
                {
                    _server.Stop();
                }
            }
        }

        /// <summary>
        /// Метод конфигурации. Вызывается первым при запуске метода Start
        /// </summary>
        private void Configuration()
        {
            Console.WriteLine("Введите подключаемый порт");
            var _portCheck = Console.ReadLine();
            if (isNumberPort(_portCheck))
            {
                _port = int.Parse(_portCheck);
            } // проверяем на правильность введения данных
            Console.WriteLine("Введите IP адрес. Например: 127.0.0.1");
            var _ipCheck = Console.ReadLine();
            if(isIPAdress(_ipCheck))
            {
                _ipAdress = _ipCheck;
            } // проверяем на правильность введения данных
            
        }

        /// <summary>
        /// Проверка на ввод числа
        /// </summary>
        /// <param name="line">Строка, где должно быть число</param>
        /// <returns></returns>
        private bool isNumberPort(string line)
        {
            try
            {
                if (line.Length > 4) // если длина порта больше 5-и символов то отправляем false
                {
                    return false;
                }
                var check = int.Parse(line);
                return true;
            }
            catch
            {
                return false;
            }
             
        }

        /// <summary>
        /// Проверка на правильность IP адреса
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool isIPAdress(string line)
        {
            var array = line.Split(new char[] { '.' });
            if (array.Length == 4)
            {
                for(int i = 0; i < array.Length; i++)
                {
                    if (!isNumberPort(array[i]))
                    {
                        return false;
                    }
                }
                return true;
            } 
            else
            {
                return false;
            }
        }

    }
}
