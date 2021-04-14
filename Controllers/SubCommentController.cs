using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Controllers
{
    public class SubCommentController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public SubCommentController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public JsonResult PostSubComment(int commentId, string text, int subCommentId = 0)
        {
            try
            {
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Id == null)
                {
                    return Json(false);
                }
                if (subCommentId != 0)
                {
                    SubComment subComment = new SubComment();
                    subComment.subCommentId = subCommentId;
                    subComment.CommentId = commentId;
                    subComment.UserId = Id;
                    subComment.User = _userManager.FindByIdAsync(Id).Result;
                    subComment.CreateDate = DateTime.Now;
                    if (subComment.UserId == null)
                    {
                        return Json(false);
                    }
                    subComment.Text = text;
                    var SubComment = _db.SubComments.Find(subCommentId);
                    SubComment.SubComments.Add(subComment);
                }
                else
                {
                    SubComment subcomment = new SubComment();
                    subcomment.CommentId = commentId;
                    subcomment.UserId = Id;
                    subcomment.Text = text;
                    var comment = _db.Comments.Find(commentId);
                    comment.SubComments.Add(subcomment);
                }
                _db.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult ChangeSubComment(int id, string text)
        {
            try
            {
                var SubComment = _db.SubComments.Include(x => x.User).First(x => x.Id == id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (SubComment.User.Id == Id)
                {
                    SubComment.Text = text;
                    _db.SaveChanges();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public IActionResult Index()
        {

            return View();
        }
        public JsonResult PostLikeSubComment(int id)
        {
            try
            {

                var comment = _db.SubComments.Find(id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    var CanceledLike = _db.LikesSubComments.Include(x => x.User).First(x => x.SubCommentId == id && x.User.Id == Id);
                    _db.LikesSubComments.Remove(CanceledLike);
                    _db.SaveChanges();
                    return Json("deleted");
                }
                catch (Exception)
                {
                    LikeSubComment sublike = new LikeSubComment();
                    sublike.SubCommentId = id;
                    sublike.UserId = Id;
                    try
                    {
                        _db.LikesSubComments.Add(sublike);
                        //comment.Likes.Add(like);

                        _db.SaveChanges();
                        return Json("added");

                    }
                    catch (Exception)
                    {

                    }

                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult DeleteSubComment(int id)
        {
            try
            {
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var DeletedSubComment = _db.SubComments.Include(x => x.User).First(x => x.Id == id);
                if (DeletedSubComment.UserId == Id)
                {
                    _db.SubComments.Remove(DeletedSubComment);
                    _db.SaveChanges();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
