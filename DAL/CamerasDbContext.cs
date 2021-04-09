using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class CamerasDbContext
    {
        public IEnumerable<Camera> GetNewCameras()
        {
            List<Camera> result = new List<Camera>();

            string connectionS = @"Data Source = RYBALKOAV\MSSQLSERVER01; Initial Catalog = CamerasDb; Integrated Security = True";
            const string query = "SELECT * FROM Cameras";

            using (SqlConnection conn = new SqlConnection(connectionS))
            {
                conn.Open();

                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cam = new Camera();
                            cam.RtspAddress = dr.GetString(1);
                            cam.Country = dr.GetString(2);
                            cam.City = dr.GetString(3);
                            cam.Latitude = (double)dr.GetSqlSingle(4);
                            cam.Longitude = (double)dr.GetSqlSingle(5);

                            result.Add(cam);
                        }
                    }
                }
            }

            return result;
        }
    }

    public class Camera
    {
        public int Id { get; set; }
        public string RtspAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
