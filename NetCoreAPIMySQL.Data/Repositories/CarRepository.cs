using Dapper;
using MySql.Data.MySqlClient;
using NetCoreAPIMySQL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public class CarRepository : ICarRepository
    {
        private MySQLConfiguration _configurationString;

        public CarRepository(MySQLConfiguration configurationString)
        {
            _configurationString = configurationString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_configurationString.ConnectionString);
        }

        public async Task<IEnumerable<Car>> GetAllCars()
        {
            var db = dbConnection();
            var sql = @"
                        select id, make, model, color, year, doors
                        from cars ";
            return await db.QueryAsync<Car>(sql, new { });
        }

        public async Task<Car> GetCarDetails(int id)
        {
            var db = dbConnection();
            var sql = @"
                        select id, make, model, color, year, doors
                        from cars 
                        where id = @id ";
            return await db.QueryFirstOrDefaultAsync<Car>(sql, new { id=id });
        }

        public async Task<bool> InsertCar(Car car)
        {
            var db = dbConnection();
            var sql = @"
                        insert into cars (make, model, color, year, doors)
                        values (@Make, @Model, @Color, @Year, @Doors) ";
            var result = await db.ExecuteAsync(sql, new { car.Make, car.Model, car.Color, car.Year, car.Doors });
            return result > 0;
        }

        public async Task<bool> UpdateCar(Car car)
        {
            var db = dbConnection();
            var sql = @"
                        update cars
                            SET make = @Make, model = @Model, color = @Color, year = @Year, doors = @Doors
                        WHERE id = @Id ";
            var result = await db.ExecuteAsync(sql, new { car.Make, car.Model, car.Color, car.Year, car.Doors, car.Id });
            return result > 0;
        }
        public async Task<bool> DeleteCar(Car car)
        {
            var db = dbConnection();
            var sql = @"
                        DELETE
                        from cars 
                        where id = @Id ";
            var result =  await db.ExecuteAsync(sql, new { id = car.Id });
            return result > 0;
        }
    }
}
