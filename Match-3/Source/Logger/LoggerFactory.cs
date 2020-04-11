using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Logger
{
    class LoggerFactory
    {
        private LoggerFactory _insatnce;

        public LoggerFactory Instance 
        {
            get
            {
                if (_insatnce == null)
                {
                    _insatnce = new LoggerFactory();
                }
                return _insatnce;
            }
        }

        protected LoggerFactory()
        {

        }

        
    }
}
