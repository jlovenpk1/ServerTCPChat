using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ClientTCPServer.Server
{
    internal class Client
    {
        private TcpClient _client;
        private Server _server;
        private NetworkStream _stream;
        private StringBuilder _builder;
        private string _userName = string.Empty;
        private byte[] _data;
        private int _bytes = 0;
        private string _message = string.Empty;
        private readonly string _messageError = "Я не знаю таких команд.....beep-beep ☺";
        private readonly string _welcomeMessage = "Вы подключились к серверу.";
        private readonly string exit = "пока";
        private readonly string clietAll = "все участники";
        private string _reponceMessage = string.Empty;
        

        public Client(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
        }

        public void WorkWithClient()
        {
            try
            {
                _stream = _client.GetStream();
                _builder = new StringBuilder();
                _data = new byte[64];
                GetUserName();
                Console.WriteLine($"Пользователь {_userName} подключился. Добро пожаловать!");
                Connect();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
                
            }
            finally
            {
                if(_stream != null)
                {
                    _stream.Close();
                }
                if (_client != null)
                {
                    _client.Close();
                    Console.WriteLine($"Клиент { _userName } отключился");
                    _server._clientList.Remove(_userName);
                }
            }
            
        }

        /// <summary>
        /// Подключиться и взаимодействовать с клиентом
        /// </summary>
        private void Connect()
        {
            while (true)
            {
                _builder.Clear();
                do
                {
                    _bytes = _stream.Read(_data, 0, _data.Length);
                    _builder.Append(Encoding.Unicode.GetString(_data, 0, _bytes));
                } while (_stream.DataAvailable);
                _message = _builder.ToString();
                _reponceMessage = ResponceClient(_message);
                if(_message == exit)
                {
                    _data = Encoding.Unicode.GetBytes(_reponceMessage);
                    _stream.Write(_data, 0, _data.Length);
                    Console.WriteLine($"{_userName} отключился!");
                    break;
                }
                else if (_message == clietAll)
                {
                    ViewAllClient();
                    Console.WriteLine($"{_userName}: {_message}");
                }
                else
                {
                    _data = Encoding.Unicode.GetBytes(_reponceMessage);
                    _stream.Write(_data, 0, _data.Length);
                    Console.WriteLine($"{_userName}: {_message}");
                }  
            }
        }

        /// <summary>
        /// Вывести всех пользователей
        /// </summary>
        private void ViewAllClient()
        {
            foreach(var client in _server._clientList)
            {
                _reponceMessage = _reponceMessage + client + "\n";
            }
            _data = Encoding.Unicode.GetBytes(_reponceMessage);
            _stream.Write(_data, 0, _data.Length);
        }

        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        private void GetUserName()
        {
            do
            {
                _bytes = _stream.Read(_data, 0, _data.Length);
                _builder.Append(Encoding.Unicode.GetString(_data, 0, _bytes));
            } while (_stream.DataAvailable);
            _userName = _builder.ToString();
            _data = Encoding.Unicode.GetBytes(_welcomeMessage + "\n");
            _stream.Write(_data, 0, _data.Length);
            _server._clientList.Add(_userName);
            foreach(var message in RoboText._roboText)
            {
                _data = Encoding.Unicode.GetBytes(message.Key+"\n");
                _stream.Write(_data, 0, _data.Length);
            }
            
        }

        /// <summary>
        /// Ответ клиенту с помощью заготовленных ответов
        /// </summary>
        /// <param name="message">запрос клиента</param>
        /// <returns></returns>
        private string ResponceClient(string message)
        {
            if (RoboText._roboText.ContainsKey(message.ToLower()))
            {
                return RoboText._roboText[message.ToLower()]+"\n";
            }
            else
            {
                return _messageError;
            }
        }
    }
}
