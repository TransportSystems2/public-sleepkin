using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pillow.PublicApi.TrackEndpoints
{
    /// <summary>
    /// Добавление трэка
    /// </summary>
    public class AddTrackRequest : BaseRequest
    {
        /// <summary>
        /// Код книги
        /// </summary>
        [FromRoute]
        public string BookCode { get; set; }
        
        /// <summary>
        /// Заголовок трэка (показывается если книга имеет несколько частей)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Является ли трэк трейлером
        /// </summary>
        public bool IsTrailer { get; set; } = false;

        /// <summary>
        /// Имеет фоновую музыку
        /// </summary>
        public bool HasBackgroundMusic { get; set; } = false;

        /// <summary>
        /// Есть проблема с автоматическим определением времени AC3 треков, поэтому выставляем их вручную
        /// </summary>
        public int DefaultDurationTimeInSeconds { get; set; } = 600;

        /// <summary>
        /// Трэк
        /// </summary>
        [Required]
        public IFormFile UploadedTrack { get; set; }
    }
}