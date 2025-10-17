using Grocery.Core.Data.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : DatabaseConnection, IProductRepository
    {
        public ProductRepository()
        {
            // Tabel aansluitend op Product.cs (Name, Stock, ShelfLife, Price)
            CreateTable(@"
                CREATE TABLE IF NOT EXISTS Product(
                    [Id]        INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Name]      NVARCHAR(80) UNIQUE NOT NULL,
                    [Stock]     INTEGER NOT NULL,
                    [ShelfLife] DATE NOT NULL,
                    [Price]     DECIMAL(10,2) NOT NULL
                )");
        }

        public List<Product> GetAll()
        {
            var result = new List<Product>();
            const string sql = @"SELECT Id, Name, Stock, date(ShelfLife), Price FROM Product;";

            OpenConnection();
            using (var cmd = new SqliteCommand(sql, Connection))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = r.GetInt32(0);
                    string name = r.GetString(1);
                    int stock = r.GetInt32(2);
                    DateOnly shelf = DateOnly.FromDateTime(r.GetDateTime(3));
                    decimal price = r.GetDecimal(4);

                    result.Add(new Product(id, name, stock, shelf, price));
                }
            }
            CloseConnection();
            return result;
        }

        public Product? Get(int id)
        {
            Product? p = null;
            const string sql = @"SELECT Id, Name, Stock, date(ShelfLife), Price FROM Product WHERE Id = @Id;";

            OpenConnection();
            using (var cmd = new SqliteCommand(sql, Connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    int pid = r.GetInt32(0);
                    string name = r.GetString(1);
                    int stock = r.GetInt32(2);
                    DateOnly shelf = DateOnly.FromDateTime(r.GetDateTime(3));
                    decimal price = r.GetDecimal(4);

                    p = new Product(pid, name, stock, shelf, price);
                }
            }
            CloseConnection();
            return p;
        }

        public Product Add(Product item)
        {
            const string sql = @"
                INSERT INTO Product (Name, Stock, ShelfLife, Price)
                VALUES (@Name, @Stock, @ShelfLife, @Price)
                RETURNING ROWID;";

            OpenConnection();
            using (var cmd = new SqliteCommand(sql, Connection))
            {
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Stock", item.Stock);
                cmd.Parameters.AddWithValue("@ShelfLife", item.ShelfLife);
                cmd.Parameters.AddWithValue("@Price", item.Price);

                item.Id = System.Convert.ToInt32(cmd.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }

        public Product? Update(Product item)
        {
            const string sql = @"
                UPDATE Product
                   SET Name      = @Name,
                       Stock     = @Stock,
                       ShelfLife = @ShelfLife,
                       Price     = @Price
                 WHERE Id = @Id;";

            OpenConnection();
            using (var cmd = new SqliteCommand(sql, Connection))
            {
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Stock", item.Stock);
                cmd.Parameters.AddWithValue("@ShelfLife", item.ShelfLife);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
            return item;
        }

        public Product? Delete(Product item)
        {
            const string sql = @"DELETE FROM Product WHERE Id = @Id;";

            OpenConnection();
            using (var cmd = new SqliteCommand(sql, Connection))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
            return item;
        }
    }
}
