using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.SharedServices.Models.CustomerOrdersModels;

namespace DataAccessLayer.CustomerOrdersDataServices
{
    public class CustomerOrdersDataService : ICustomerOrdersDataService
    {
        private string _ConnectionSrting;
        private readonly IConfiguration _configuration;

        public CustomerOrdersDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnectionSrting = _configuration.GetConnectionString("DataConnection");
        }

        public static List<T> ConvertDatatableToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            if (row[pro.Name] != DBNull.Value)
                                pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to convert " + pro.Name + " datatable to model list" + ex.Message);
                        }
                    }
                }
                return objT;
            }).ToList();
        }

        public async Task<CustomerOrderModel> GetRecentOrders(string user, string customerId)
        {
            try
            {
                CustomerOrderModel result = new CustomerOrderModel();

                using (SqlConnection con = new SqlConnection(_ConnectionSrting))
                {
                    SqlCommand cmd = new SqlCommand("CUST_GetCustomerOrders", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    #region SQL Parameter
                    
                    if (!string.IsNullOrEmpty(user))
                    {
                        cmd.Parameters.AddWithValue("@user", user);
                    }

                    if (!string.IsNullOrEmpty(customerId))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                    }
                    #endregion
                    con.Open();
                    DataSet dataset = new DataSet();
                    da.Fill(dataset);
                    result.Customer = ConvertDatatableToList<Customer>(dataset.Tables[0])[0];
                    result.Order = ConvertDatatableToList<Order>(dataset.Tables[1])[0];
                    result.Order.OrderItems = ConvertDatatableToList<OrderItem>(dataset.Tables[2]);
                    con.Close();
                    
                    return await Task.FromResult(result);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
