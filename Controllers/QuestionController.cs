using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.DTO;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public QuestionController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<JsonResult> PostQuestion(int topicId, string title, string description)//QuestionDTO1 question)
        {
            try
            {
                Question q = new Question();
                q.Title = title;
                q.Description = description;
                q.CreatedDate = DateTime.Now;
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                q.User = await _userManager.FindByIdAsync(id);
                q.Views = 0;
                q.Topic = _db.Topics.Find(topicId);
                _db.Questions.Add(q);
                _db.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public JsonResult GetQuestions(int topicId = -1)
        {
            try
            {
                if (topicId == -1)
                {
                    return Json(_db.Questions.ToList());
                }
                else
                {
                    return Json(_db.Questions.Where(x => x.TopicId == topicId).ToList());
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult ChangeQuestion(int id, string title, string description)
        {
            try
            {
                if (title == null || description == null)
                {
                    return Json(false);
                }
                var question = _db.Questions.Include(x => x.User).First(x => x.Id == id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Id == null)
                {
                    return Json(false);
                }
                if (question.User.Id == Id)
                {
                    question.Title = title;
                    question.Description = description;
                    _db.SaveChanges();
                    return Json(true);
                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult OrderQuestionsByDate(IEnumerable<Question> questions, bool descending = true)
        {
            try
            {
                if (descending)
                {
                    var result = questions.OrderByDescending(x => x.CreatedDate);
                    return Json(new { data = result });
                }
                else
                {
                    var result = questions.OrderBy(x => x.CreatedDate);
                    return Json(new { data = result });
                }

            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public JsonResult GetQuestionsOrderByDate(bool descending = true, int topicId = -1)
        {
            try
            {
                if (topicId == -1)
                {
                    return OrderQuestionsByDate(_db.Questions, descending);
                }
                else
                {
                    return OrderQuestionsByDate(_db.Questions.Where(x => x.TopicId == topicId), descending);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public JsonResult GetQuestionsByUser(string userId, bool descending = true)
        {
            try
            {
                return OrderQuestionsByDate(_db.Questions.Where(x => x.UserId == userId), descending);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public JsonResult GetQuestionById(int id)
        {
            try
            {
                Question question = _db.Questions.Include(x => x.Comments).First(x => x.Id == id);
                ICollection<CommentDTO1> comments = new List<CommentDTO1>();

                foreach (var comment in question.Comments)
                {
                    var com = _db.Comments.Include(x => x.SubComments).Include(x => x.user).Include(x => x.Likes).First(x => x.Id == comment.Id);
                    UserDTO1 user = null;
                    try
                    {
                        user = new UserDTO1 { Id = com.user.Id, UserName = com.user.UserName };

                    }
                    catch (Exception)
                    {
                    }
                    ICollection<SubCommentDTO1> subComments = new List<SubCommentDTO1>();
                    foreach (var sub in com.SubComments)
                    {
                        var username = _userManager.FindByIdAsync(sub.UserId).Result.UserName;
                        var us = new UserDTO1 { Id = sub.User.Id, UserName = username };
                        var likes = _db.LikesSubComments.Where(x => x.SubCommentId == sub.Id).Count();
                        var newsub = new SubCommentDTO1 { Id = sub.Id, Text = sub.Text, User = us, CreateDate = sub.CreateDate, Likes = likes };
                        subComments.Add(newsub);
                    }
                    subComments.OrderBy(x => x.CreateDate);
                    var comdto = new CommentDTO1 { Id = com.Id, Changed = com.Changed, CreatedDate = com.CreatedDate, IsAnswer = com.IsAnswer, Likes = com.Likes.Count, Text = com.Text, user = user, SubComments = subComments };

                    comments.Add(comdto);
                }
                return Json(new
                {
                    question = _db.Questions.Include(x => x.User)
                    .Select(x => new
                    {
                        x.UserId,
                        _userManager.FindByIdAsync(x.UserId).Result.UserName,
                        x.Id,
                        x.CreatedDate,
                        //comments = x.Comments.Select(y => new { y.Text, userId = _db.Comments.Include(u => u.user).First(x => x.Id == y.Id).user.Id, _userManager.FindByIdAsync(_db.Comments.Include(u => u.user).First(x => x.Id == y.Id).user.Id).Result.UserName, y.CreatedDate, y.Changed, y.IsAnswer, likes = y.Likes.Count, y.Id }),
                        //comments = _db.Comments.Include(z => z.SubComments).Include(z => z.Question).Include(z => z.user).Where(z => z.Question.Id == x.Id).Select(y => new { y.Text, userId = y.user.Id, _userManager.FindByIdAsync(y.user.Id).Result.UserName, y.CreatedDate, y.Changed, y.IsAnswer, likes = y.Likes.Count, y.Id, subComments = y.SubComments }),
                        x.Title,
                        x.TopicId,
                        x.Description,
                        x.Views,
                        savesCount = x.UsersSaved.Count
                    })
                    .First(x => x.Id == id),
                    comments = comments
                });
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetMyLastQuestion()
        {
            try
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Json(await _db.Questions.Where(x => x.UserId == id).OrderBy(x => x.CreatedDate).FirstAsync());
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User u = await _userManager.FindByIdAsync(userId);
            ViewBag.User = u;
            return View();
        }
        public JsonResult AddSavedQuestion(int id = -1)
        {
            try
            {
                if (id != -1)
                {
                    SavedQuestion re = null;
                    string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    try
                    {
                        re = _db.SavedQuestions.Include(x => x.User).Include(x => x.Questions).First(x => x.User.Id == Id);
                    }
                    catch (Exception)
                    {
                        // return Json(new { data = "exception" });
                    }
                    if (re != null)
                    {
                        try
                        {
                            var del = re.Questions.First(x => x.Id == id);

                            re.Questions.Remove(del);
                            _db.SaveChanges();
                            return Json(new { data = "deleted" });

                        }
                        catch (Exception)
                        {
                            re.Questions.Add(_db.Questions.First(x => x.Id == id));
                            _db.SaveChanges();
                            return Json(new { data = "added" });

                        }


                    }
                    else
                    {
                        SavedQuestion sq = new SavedQuestion();
                        sq.User = _userManager.FindByIdAsync(Id).Result;
                        sq.Questions.Add(_db.Questions.First(x => x.Id == id));
                        sq.UserId = Id;
                        _db.SavedQuestions.Add(sq);
                        _db.SaveChanges();

                    }
                    return Json(new { data = "all okey!" });
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

        public JsonResult ShowSavedQuestions(string userId)
        {
            try
            {
                if (userId != null)
                {
                    return Json(_db.SavedQuestions.Include(x => x.User).Include(x => x.Questions).Select(x => new { x.Id, x.UserId, question = x.Questions.Select(y => new { y.Title, y.Id }) }).First(x => x.UserId == userId));
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
