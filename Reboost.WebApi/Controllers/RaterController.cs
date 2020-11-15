using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;
using Reboost.WebApi.Models;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaterController : BaseController<IRaterService>
    {
        private readonly IMapper _mapper;

        public RaterController(IRaterService service, IMapper mapper) : base(service)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Rater>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Rater> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Rater> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<Rater> CreateAsync([FromForm] RaterModel model)
        {
            try
            {
                var _rater = _mapper.Map<Rater>(model);

                _rater.Scores = JsonConvert.DeserializeObject<List<UserScore>>(model.ScoreJSON);

                if(model.IELTSCertificatePhotos != null)
                {
                    foreach (var photo in model.IELTSCertificatePhotos)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            var _photo = new Photo
                            {
                                File = memoryStream.ToArray(),
                                FileName = photo.FileName,
                                FileType = "IELS Certificate"
                            };

                            _rater.Photos.Add(_photo);
                        }
                    }
                }
                if (model.TOEFLCertificatePhotos != null)
                {
                    foreach (var photo in model.TOEFLCertificatePhotos)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            var _photo = new Photo
                            {
                                File = memoryStream.ToArray(),
                                FileName = photo.FileName,
                                FileType = "TOEFL Certificate"
                            };

                            _rater.Photos.Add(_photo);
                        }
                    }
                }
                if (model.IDCardPhotos != null)
                {
                    foreach (var photo in model.IDCardPhotos)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            var _photo = new Photo
                            {
                                File = memoryStream.ToArray(),
                                FileName = photo.FileName,
                                FileType = "ID"
                            };

                            _rater.Photos.Add(_photo);
                        }
                    }
                }


                return await _service.CreateAsync(_rater);
            }
            catch (Exception ex)
            {
                return await Task.FromException<Rater>(ex);
            }
        }
        [HttpPost]
        [Route("update")]
        public async Task<Rater> UpdateAsync([FromForm] RaterModel model)
        {
            var _rater = _mapper.Map<Rater>(model);
            var _photos = new List<Photo>();
            foreach (var photo in model.IELTSCertificatePhotos)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    var _photo = new Photo
                    {
                        File = memoryStream.ToArray(),
                        FileName = photo.FileName,
                        FileType = "Certificate"
                    };

                    _photos.Add(_photo);
                }
            }
            foreach (var photo in model.TOEFLCertificatePhotos)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    var _photo = new Photo
                    {
                        File = memoryStream.ToArray(),
                        FileName = photo.FileName,
                        FileType = "IdPhoto"
                    };

                    _photos.Add(_photo);
                }
            }
            foreach (var photo in model.IDCardPhotos)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    var _photo = new Photo
                    {
                        File = memoryStream.ToArray(),
                        FileName = photo.FileName,
                        FileType = "IdPhoto"
                    };

                    _photos.Add(_photo);
                }
            }

            _rater.Photos = _photos;
            return await _service.UpdateAsync(_rater);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadModel data) {

            //data.File.
            return Ok(await Task.FromResult(new { Result = "Success" }));
        }
    }

    public class UploadModel {
        [FromForm(Name = "FileName")]
        public string FileName { get; set; }
        [FromForm(Name = "Files")]
        public List<IFormFile> Files { get; set; }
    }
}
