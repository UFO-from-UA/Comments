using Comments.Database;
using Comments.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Comments.Controllers
{
    public class HomeController : Controller
    {
        DB _db =  new DB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProdComment()
        {
            try
            {
                var tmp = /*await*/ _db.Comment;
                //.Where(x => x)
                //.ToListAsync();
                List<Models.Comment> ListComment = new List<Models.Comment>();
                //Можно сократить
                foreach (var it in tmp)
                {
                    if (it.Parent != null)
                    {
                        var ParentIndex = ListComment.IndexOf(ListComment.Where(x => x.Id == it.Parent).FirstOrDefault());
                        if (ParentIndex + 1 < ListComment.Count)
                        {
                            if (ListComment[ParentIndex + 1].LVL != "0rem")
                            {
                                int i = 1;
                                while (true)
                                {
                                    if (ListComment[ParentIndex + i].LVL[0].ToString() == "0") { break; }
                                    if (int.Parse(ListComment[ParentIndex + i].LVL[0].ToString()) / 4 != it.LVL && int.Parse(ListComment[ParentIndex + i].LVL[0].ToString()) / 4 < it.LVL)
                                    {
                                        i++;
                                        #region Убирает LIFO sort на всех дочерних
                                        //Убирает LIFO sort на всех дочерних
                                        //if (int.Parse(ListComment[ParentIndex + i].LVL[0].ToString()) / 4 == it.LVL)
                                        //{
                                        //    var j = 1;
                                        //    while (int.Parse(ListComment[ParentIndex + i+j].LVL[0].ToString()) / 4 == it.LVL)
                                        //    {
                                        //        j++;
                                        //    }
                                        //    i += j;
                                        //}
                                        #endregion
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                ListComment.Insert(i + ParentIndex, new Models.Comment()
                                {
                                    Id = it.Id_Comment,
                                    DateComment = it.DateComment,
                                    Title = it.Title,
                                    Message = it.Message,
                                    LVL = (it.LVL * 4).ToString() + "rem",
                                    UserName = it.User.UserName,
                                    Email = it.User.Email,
                                    HomePage = it.User.HomePage ?? "",
                                    //IpAddres = it.User.IpAddres.ToString()
                                });
                            }
                            else
                            {
                                ListComment.Insert(1 + ParentIndex, new Models.Comment()
                                {
                                    Id = it.Id_Comment,
                                    DateComment = it.DateComment,
                                    Title = it.Title,
                                    Message = it.Message,
                                    LVL = (it.LVL * 4).ToString() + "rem",
                                    UserName = it.User.UserName,
                                    Email = it.User.Email,
                                    HomePage = it.User.HomePage ?? "",
                                    //IpAddres = it.User.IpAddres.ToString()
                                });
                            }
                        }
                    }
                    else
                    {
                        ListComment.Add(new Models.Comment()
                        {
                            Id = it.Id_Comment,
                            DateComment = it.DateComment,
                            Title = it.Title,
                            Message = it.Message,
                            LVL = (it.LVL * 4).ToString() + "rem",
                            UserName = it.User.UserName,
                            Email = it.User.Email,
                            HomePage = it.User.HomePage ?? "",
                            //IpAddres = it.User.IpAddres.ToString()
                        });
                    }
                }
                ViewBag.ListItems = ListComment;
                return View();
            }
            catch (Exception ex )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }
           
        }
        public ActionResult Error()
        {
            return View(); 
        }
        //public ActionResult Publish()
        //{
        //    return RedirectToAction("Error", "Home");
        //}
        [HttpPost]
        public ActionResult Publish(Models.Comment data)
        {
            if (data.Captcha != (string)Session["capcha"])
            {
                //var script = "window.location ='" + Url.Action("Error", "Home") + "' ;";
                //return JavaScript(script);
                return RedirectToAction("Error", "Home");
            }
            if (ServerCheck(ref data))
            {
                InsertIntoDB(data);
            }
            else
            { // Редирект с поста  почемуто не  работает 
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ProdComment", "Home"); 
        }

        private void InsertIntoDB(Models.Comment data)
        {
            try
            {
                _db.IpAddres.Add(new IpAddres() { ClientIp = data.IpAddres });
                _db.SaveChanges();
                var IP = _db.IpAddres.Where(x => x.ClientIp == data.IpAddres).FirstOrDefault();
                //Много умных проверок на посещение с этого IP и даленейшее связывание в  БД
                if (IP != null)
                {
                    _db.User.Add(new Database.User() { UserName = data.UserName, Email = data.Email, HomePage = data.HomePage, Id_ip = IP.Id_ip });
                }
                _db.SaveChanges();
                var User = _db.User.Where(x => x.Email == data.Email).FirstOrDefault();
                if (User != null)
                {
                    _db.Comment.Add(new Database.Comment()
                    {
                        Id_User = User.Id_User,
                        DateComment = data.DateComment ?? Convert.ToDateTime(data.DateComment),
                        User = User,
                        Title = data.Title,
                        LVL = int.Parse(data.LVL)+1,
                        Message = data.Message,
                        Parent = data.Parent_Id
                    });
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        private bool ServerCheck(ref Models.Comment data)
        {
           
            if (data.Title==null && data.Title.Length<3)
            {                return false;            }
            if (String.IsNullOrEmpty(data.UserName))
            { return false; }
            if (String.IsNullOrEmpty(data.Email))
            { return false; }
            if (String.IsNullOrEmpty(data.Message))
            { return false; }
            if (!IsValidEmail(data.Email))
            { return false; }
            data.DateComment = DateTime.Now;
            data.IpAddres = GetIP();
            return true;
        }

        public ActionResult Captcha()
        {
            string code = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
            Session["capcha"] = code;
            CaptchaImage captcha = new CaptchaImage(code, 110, 50);

            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            captcha.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            captcha.Dispose();
            return null;
        }

        public string GetIP()
        {
            return IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                                                            Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                            Request.ServerVariables["REMOTE_ADDR"]); ;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}