using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreUtilidades.Helpers
{
    public class HelperMail
    {
        private IConfiguration configuration;

        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
