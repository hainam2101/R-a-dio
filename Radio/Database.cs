using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data.SQLite;

namespace Radio
{
    class Database
    {
        #region Database File Path

        public static readonly string CurrentPath = Directory.GetCurrentDirectory() + "\\";
        const string _DBFile = "favorites.sqlite";
        static readonly string _fullDBPath = CurrentPath + _DBFile;

        #endregion // Database File Path

        #region Constructors


        #endregion // Constructors

        #region Columns & Table

        const string Table = "Music";
        const string Song_ID = "Song_ID";
        const string Name = "Name";
        const string Favorite = "Favorite";

        #endregion // Columns & Table

        #region Parameters

        const string Song = "@Song";
        const string Fav = "@Fav";
        const string ID = "@ID";

        #endregion // Parameters

        #region Database Operations
        public static bool ExistsDB()
        {
            return File.Exists(_fullDBPath);
        }

        public static void CreateDBFileAndTable()
        {
            SQLiteConnection.CreateFile(_fullDBPath);

            var dbConn = CreateDBConnection();
            dbConn.Open();

            CreateTable(dbConn);

            dbConn.Close();
        }

        /// <summary>
        /// Creates the DB connection. It's user's responsibility to Open and Close this connection.
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection CreateDBConnection()
        {
            return new SQLiteConnection("Data Source=" + _fullDBPath + ";Version=3;");

            // Exception in Open(): Data Source cannot be empty.  Use :memory: to open an in-memory database
            //m_DBConnection = new SQLiteConnection(currentPath  + "=" + DBFile + ";Version=3;");
        }

        /// <summary>
        /// Insert's a new song favorite. The column Favorite is set to True by default.
        /// </summary>
        /// <param name="name">Song's name.</param>
        /// <param name="conn">An opened DB connection.</param>
        public static void InsertRecord(string name, SQLiteConnection conn)
        {
            var cmd = String.Format("INSERT INTO {0} ({1}, {2}) VALUES({3}, {4})",
                Table, Name, Favorite, Song, Fav);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.Parameters.Add(new SQLiteParameter(Song, name));
            sqliteCMD.Parameters.Add(new SQLiteParameter(Fav, 1));
            sqliteCMD.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the song's Favorite column.
        /// </summary>
        /// <param name="name">Song's name.</param>
        /// <param name="favorite">Favorite boolean.</param>
        /// <param name="conn">An opened DB connection.</param>
        public static void UpdateRecord(string name, bool favorite, SQLiteConnection conn)
        {
            var cmd = String.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}",
                Table, Favorite, Fav, Name, Song);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.Parameters.Add(new SQLiteParameter(Song, name));
            sqliteCMD.Parameters.Add(new SQLiteParameter(Fav, favorite.ToString()));
            sqliteCMD.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a record based in name.
        /// </summary>
        /// <param name="name">Song's name.</param>
        /// <param name="conn">An opened DB connection.</param>
        public static void DeleteRecord(string name, SQLiteConnection conn)
        {
            var cmd = String.Format("DELETE FROM {0} WHERE {1} = {2}",
                Table, Name, Song);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.Parameters.Add(new SQLiteParameter(Song, name));
            sqliteCMD.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a record based in the ID.
        /// </summary>
        /// <param name="currID">Curent ID.</param>
        /// <param name="conn">An opened DB connection.</param>
        public static void DeleteRecord(int currID, SQLiteConnection conn)
        {
            var cmd = String.Format("DELETE FROM {0} WHERE {1} = {2}",
                Table, Song_ID, ID);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.Parameters.Add(new SQLiteParameter(ID, currID));
            sqliteCMD.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates the main table.
        /// </summary>
        /// <param name="conn">An opened DB connection.</param>
        /*public static void CreateTable(SQLiteConnection conn) // version that uses Varchar (though, SQLite always uses TEXT I think)
        {
            var cmd = String.Format("CREATE TABLE {0} ({1} VARCHAR(200), {2} bool)",
                Table, Name, Favorite);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.ExecuteNonQuery();
        }*/
        // Creating the version with the ID (autoincrement) adn testing UTF-16LE C# strings
        public static void CreateTable(SQLiteConnection conn)
        {
            var cmd = String.Format("CREATE TABLE {0} ({1} INTEGER PRIMARY KEY AUTOINCREMENT, {2} TEXT, {3} BOOL)",
                Table, Song_ID, Name, Favorite);
            var sqliteCMD = new SQLiteCommand(cmd, conn);
            sqliteCMD.ExecuteNonQuery();
        }

        public static void PrintAllRecords(SQLiteConnection conn)
        {
            var sqlSelect = String.Format("SELECT * FROM {0}", Table);
            var sqliteCMD = new SQLiteCommand(sqlSelect, conn);
            var reader = sqliteCMD.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("ID: " + reader[Song_ID] + "\tSong: " + reader[Name] + "\tFavorite: " + reader[Favorite]);
            }
        }

        #endregion // Database Operations
    }
}
