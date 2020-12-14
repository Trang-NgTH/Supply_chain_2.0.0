using Olwen_2._0._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.DependencyInjection
{
    public class DbSupport
    {
        private static DbSupport _ins;

        public static DbSupport Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DbSupport();
                return _ins;
            }

            set
            {
                _ins = value;
            }
        }

        public DbEntities Db;

        public DbSupport()
        {
            Db = new DbEntities();
        }
    }
}
