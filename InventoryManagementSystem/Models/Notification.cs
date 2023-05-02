using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Notificationn { get; set; }
        public int ManagementId { get; set; }
        public int Isview { get; set; }
        public int IsDelete { get; set; }

        public virtual Management Management { get; set; }
    }
}
