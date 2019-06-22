using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.DAL.Gateway
{
    class BaseGateway
    {
        public SqlConnection Connection;

        public BaseGateway()
        {
            Connection = new SqlConnection(AppVariable.ServerLogin);
            //Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ThisConnectionString"].ConnectionString);
        }
    }
}
