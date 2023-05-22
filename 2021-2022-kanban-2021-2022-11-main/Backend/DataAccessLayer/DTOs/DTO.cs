using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    /// <summary>
    /// A class of DTO. 
    /// </summary
    internal abstract class DTO
    {
        protected DALcontroller _controller;
        protected DTO(DALcontroller controller)
        {
            _controller = controller;
        }
    }
}
