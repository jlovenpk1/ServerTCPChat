using System;
using System.Collections.Generic;
using System.Text;

namespace ClientTCPServer.Server
{
    internal static class RoboText
    {
        internal static Dictionary<string, string> _roboText = new Dictionary<string, string>() { { "привет", "beep-beep Добрый вечер или день? Здравствуйте." },
                                                                                        { "как дела?", "beep-beep нужно срочно почистить мою память, а в целом отлично." },
                                                                                        { "пока", "И Вам всего хорошего." },
                                                                                        { "все участники", "Отлично, вот список участников:" } };
    }
}
