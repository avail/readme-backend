using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class TemplatingService
    {
        public TemplatingService()
        {

        }

        // basic replacement
        public string Perform(string template, string field, string value)
        {
            return template.Replace("{{ " + field + " }}", value);
        }

        // TODO: more complex replacement (whole element insertion with templates defined in svg itself?)
    }
}
