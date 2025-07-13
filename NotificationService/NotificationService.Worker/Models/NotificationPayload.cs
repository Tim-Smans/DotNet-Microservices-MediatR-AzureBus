using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Worker.Models
{
    public record NotificationPayload(string Email, string Message);
}
