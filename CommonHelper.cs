using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HSMS.UI
{
   public class CommonHelper
    {
       public static Guid GetOperatorId()
       {
           Guid operatorId = (Guid)Application.Current.Properties["OperatorID"];
           return operatorId;
       }
    }
}
