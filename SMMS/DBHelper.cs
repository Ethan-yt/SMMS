using SMMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMMS
{
    static class DBHelper
    {
        private static SQLiteConnection conn = null;
        public static User currentUser = null;
        internal static string MD5(string text)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(text)));
        }
        public static SQLiteTransaction beginTransaction()
        {
            return conn.BeginTransaction();
        }
        public static void initDB()
        {
            string dbPath = "Data Source = " + Environment.CurrentDirectory + "/database.db";
            conn = new SQLiteConnection(dbPath);
            conn.Open();
        }

        internal static void addUser()
        {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = "INSERT INTO USER(UNAME,PASSWORD,GID) VALUES('新用户','1B2M2Y8AsgTpgAmY7PhCfg==','0')";
            cmd.ExecuteNonQuery();
            addLog(currentUser.UID.ToString(),"增加新用户");

        }
        internal static void addGroup()
        {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = "INSERT INTO `GROUP`(NAME) VALUES('新用户组')";
            cmd.ExecuteNonQuery();
            addLog(currentUser.UID.ToString(), "增加新用户组");
        }
        internal static int addGoods(string gNAME, string pRICE, string cATEGORY, string uNIT, string cODE)
        {
            float f;
            if (string.IsNullOrEmpty(gNAME) || string.IsNullOrEmpty(pRICE) || !float.TryParse(pRICE, out f))
                throw new Exception("数据格式错误");
            var t = conn.BeginTransaction();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = "INSERT INTO GOODS(GNAME,PRICE,CATEGORY,UNIT,CODE) VALUES(@GNAME,@PRICE,@CATEGORY,@UNIT,@CODE)";
            cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@GNAME", gNAME),
                           new SQLiteParameter("@PRICE", pRICE),
                           new SQLiteParameter("@CATEGORY", cATEGORY),
                           new SQLiteParameter("@UNIT", uNIT),
                           new SQLiteParameter("@CODE", cODE),
                       });
            cmd.ExecuteNonQuery();
            addLog(currentUser.UID.ToString(), "增加商品："+gNAME);
            cmd.CommandText = "SELECT seq FROM sqlite_sequence WHERE name = 'GOODS'";

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        t.Commit();
                        return reader.GetInt32(0);
                    }
                }
            }

            t.Rollback();
            throw new Exception("获取货号失败");

        }
        internal static void addLog(string uid, string command)
        {
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(command))
                throw new Exception("数据格式错误");

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = "INSERT INTO LOG(UID,COMMAND,TIME) VALUES(@UID,@COMMAND,datetime('now','localtime'))";
            cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@UID", uid),
                           new SQLiteParameter("@COMMAND", command),
                       });
            cmd.ExecuteNonQuery();


        }


        public static User login(string username, string password)
        {
            string md5Password = MD5(password);
            var users = getUser("UNAME = '" + username + "' AND PASSWORD = '" + md5Password + "'",false);
            if (users != null && users.Count == 1)
            {
                addLog("0", "用户登录成功："+username);
                return users[0];
            }
            else
            {
                addLog("0", "用户登录失败 尝试使用用户名：" +username +" 密码："+ password );
                return null;
            }
               
        }


        public static List<User> getUser(string where = "", bool log = true)
        {
            List<User> ret = new List<User>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from USER";
                if (!String.IsNullOrEmpty(where))
                    cmd.CommandText += " WHERE " + where;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetValue(4) as string, reader.GetValue(5) as string, reader.GetValue(6) as string));
                    }
                }
            }
            if (String.IsNullOrEmpty(where))
                where = "(空)";
            if(log)
                addLog(currentUser.UID.ToString(), "查询用户 条件：" + where);
            return ret;
        }
        
        public static string getLastLoginTime(string username)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT TIME FROM LOG WHERE COMMAND = '用户登录成功："+username+ "' ORDER BY TIME DESC LIMIT 1 OFFSET 1 ";
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetValue(0) as string;
                    }
                }
            }
            return null;
        }
        public static List<Log> getLog(string where = "", bool log = true)
        {
            List<Log> ret = new List<Log>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select LOG.ID,LOG.UID,USER.UNAME,LOG.COMMAND,LOG.TIME from LOG,USER WHERE LOG.UID = USER.UID";
                if (!String.IsNullOrEmpty(where))
                    cmd.CommandText += " AND " + where;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new Log(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetValue(3) as string, reader.GetValue(4) as string));
                    }
                }
            }
            if (String.IsNullOrEmpty(where))
                where = "(空)";
            if (log)
                addLog(currentUser.UID.ToString(), "查询日志 条件：" + where);
            return ret;
        }

        public static List<Goods> getGoods(string where = "", bool log = true)
        {
            List<Goods> ret = new List<Goods>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from GOODS";
                if (!String.IsNullOrEmpty(where))
                    cmd.CommandText += " WHERE " + where;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new Goods(
                            reader.GetInt32(0),
                            reader.GetValue(1) as string,
                            reader.GetFloat(2),
                            reader.GetValue(3) as string,
                            reader.GetValue(4) as string,
                            reader.GetInt32(5),
                            reader.GetValue(6) as string
                            ));
                    }
                }

            }
            if (String.IsNullOrEmpty(where))
                where = "(空)";
            if (log)
                addLog(currentUser.UID.ToString(), "查询货物 条件：" + where);
            return ret;
        }
        public static List<Group> getGroup(string where = "", bool log = true)
        {
            List<Group> ret = new List<Group>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM `GROUP`";
                if (!String.IsNullOrEmpty(where))
                    cmd.CommandText += " WHERE " + where;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new Group(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetBoolean(5), reader.GetBoolean(6), reader.GetBoolean(7), reader.GetBoolean(8), reader.GetBoolean(9), reader.GetBoolean(10)));
                    }
                }
            }
            if (String.IsNullOrEmpty(where))
                where = "(空)";
            if(log)
                addLog(currentUser.UID.ToString(), "查询用户组 条件：" + where);
            return ret;
        }
        public static List<object[]> getSummary()
        {
            List<object[]> ret = new List<object[]>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT SUM(NUM) ,CATEGORY FROM GOODS GROUP BY CATEGORY";
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new object[] {
                            reader.GetInt32(0),
                            reader.GetValue(1),
                        });
                    }
                }

            }
            return ret;
        }

        internal static void updateUser(int uID, string propertyName, string v)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "UPDATE USER SET " + propertyName + " = @VALUE WHERE UID = @UID";
                cmd.Parameters.AddRange(new[] {
                           //new SQLiteParameter("@PROPERTYNAME", propertyName),
                           new SQLiteParameter("@VALUE", v),
                           new SQLiteParameter("@UID", uID)
                       });
                addLog(currentUser.UID.ToString(), "修改用户 用户ID："+uID+" 属性：" + propertyName+" 值："+v);
                cmd.ExecuteNonQuery();
            }
        }
        internal static void updateGroup(int id, string propertyName, string v)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "UPDATE `GROUP` SET " + propertyName + " = @VALUE WHERE ID = @ID";
                cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@VALUE", v),
                           new SQLiteParameter("@ID", id)
                       });
                addLog(currentUser.UID.ToString(), "修改用户组 用户组ID："+ id + " 属性：" + propertyName + " 值：" + v);
                cmd.ExecuteNonQuery();
            }
        }

        internal static void updateGoods(int gID, string propertyName, string v, bool restock = false, bool sale = false, int num = 0)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "UPDATE GOODS SET " + propertyName + " = @VALUE WHERE GID = @GID";
                cmd.Parameters.AddRange(new[] {
                           //new SQLiteParameter("@PROPERTYNAME", propertyName),
                           new SQLiteParameter("@VALUE", v),
                           new SQLiteParameter("@GID", gID)
                       });
                if(restock)
                    addLog(currentUser.UID.ToString(), "进货 货号："+gID+" 数量："  + num + " 当前库存：" + v);
                else
                    addLog(currentUser.UID.ToString(), "销售 货号：" + gID + " 数量：" + num + " 当前库存：" + v);
                cmd.ExecuteNonQuery();
            }
        }

        internal static void deleteGoods(int gID)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "DELETE FROM GOODS WHERE GID = @GID";
                cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@GID", gID)
                       });
                addLog(currentUser.UID.ToString(), "删除货物 货号：" + gID);
                cmd.ExecuteNonQuery();
            }
        }
        internal static void deleteUser(int uID)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "DELETE FROM USER WHERE UID = @UID";
                cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@UID", uID)
                       });
                addLog(currentUser.UID.ToString(), "删除用户 ID：" + uID);
                cmd.ExecuteNonQuery();
            }
        }
        internal static void deleteLog(int id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "DELETE FROM LOG WHERE ID = @ID";
                cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@ID", id)
                       });
                addLog(currentUser.UID.ToString(), "删除日志 日志号：" + id);
                cmd.ExecuteNonQuery();
            }
        }
        internal static void deleteGroup(int id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "DELETE FROM `GROUP` WHERE ID = @ID";
                cmd.Parameters.AddRange(new[] {
                           new SQLiteParameter("@ID", id)
                       });
                addLog(currentUser.UID.ToString(), "删除用户组 用户组ID：" + id);
                cmd.ExecuteNonQuery();
            }
        }

        public static string[] getCategorys()
        {
            List<string> ret = new List<string>();
            SQLiteCommand cmd = new SQLiteCommand(conn);

            cmd.CommandText = "SELECT DISTINCT CATEGORY FROM GOODS";
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ret.Add(reader.GetValue(0) as string);
                }
            }
            return ret.ToArray();
        }



    }
}
