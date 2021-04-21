using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ChatMessagePagination
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int ChatId{ get; set; }

    }
}
