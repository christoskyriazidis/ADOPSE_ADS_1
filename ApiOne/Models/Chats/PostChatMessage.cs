using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class PostChatMessage
    {
       
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string MessageText{ get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ActiveChat { get; set; }
    }
}
