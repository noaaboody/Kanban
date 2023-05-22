using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }
        //public bool ErrorOccured { get => ErrorMessage != null; }
        internal Response()
        {
            ErrorMessage = null;
            ReturnValue = null;
        }
        internal Response(string msg)
        {
            ErrorMessage = msg;
            ReturnValue = null;
        }
        
        internal void ResponseObj(object obj)
        {
            ReturnValue = obj;
            ErrorMessage = null;
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            return JsonSerializer.Serialize(this, this.GetType(), options);
        }
    }
}
