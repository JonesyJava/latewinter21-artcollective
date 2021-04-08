using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using latewinter_artcollective.Models;

namespace latewinter_artcollective.Repositories
{
  public class PaintingsRepository
  {
    private readonly IDbConnection _db;

    public PaintingsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal IEnumerable<Painting> GetAll()
    {
      string sql = "SELECT * FROM paintings;";
      return _db.Query<Painting>(sql);
    }

    internal Painting GetById(int id)
    {
      string sql = "SELECT * FROM paintings WHERE id = @id;";
      return _db.QueryFirstOrDefault<Painting>(sql, new { id });
    }

    internal Painting Create(Painting newPainting)
    {
      string sql = @"
      INSERT INTO paintings
      (title, description, year, artistId)
      VALUES
      (@Title, @Description, @Year, @ArtistId);
      SELECT LAST_INSERT_ID();";
      int id = _db.ExecuteScalar<int>(sql, newPainting);
      newPainting.Id = id;
      return newPainting;
    }

    internal Painting Edit(Painting data)
    {
      string sql = @"
      UPDATE paintings
      SET
        title = @Title,
        description = @Description,
        year = @Year
      WHERE id = @Id;
      SELECT * FROM paintings WHERE id = @Id;";
      return _db.QueryFirstOrDefault<Painting>(sql, data);
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM paintings WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
    }

    internal IEnumerable<Painting> GetByArtistId(int id)
    {
      string sql = "SELECT * FROM paintings WHERE artistId = @id;";

      return _db.Query<Painting>(sql, new { id });
    }
  }
}