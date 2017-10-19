﻿using myNote.DataLayer;
using myNote.DataLayer.Sql;
using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace myNote.Api.Controllers
{
    /// <summary>
    /// Управление группами заметок
    /// </summary>
    public class NoteGroupController : ApiController
    {
        private NoteGroupsRepository noteGroupsRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public NoteGroupController()
        {
            noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
        }
        /// <summary>
        /// Создание группы заметок
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="groupId">Идентификатор группы</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/groups/{groupId}/notes/{noteId}")]
        public NoteGroup Post(Guid noteId, Guid groupId)
        {
            return noteGroupsRepository.CreateNoteGroup(noteId, groupId);
        }
        /// <summary>
        /// Получение группы заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/notes/{id}/group")]
        public Group Get(Guid id)
        {
            return noteGroupsRepository.GetGroupBy(id);
        }

        /// <summary>
        /// Получение всех заметок в группе
        /// </summary>
        /// <param name="groupId">Идентификатор группы</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/groups/{groupId}/notes")]
        public IEnumerable<Note> GetAlNotesBy(Guid groupId)
        {
            return noteGroupsRepository.GetAllNoteBy(groupId);
        }

        /// <summary>
        /// Получение всех заметок в группе
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/{userId}/groups/{name}/notes")]
        public IEnumerable<Note> GetAlNotesBy(Guid userId, string name)
        {
            return noteGroupsRepository.GetAllNoteBy(userId, name);
        }
    }
}
