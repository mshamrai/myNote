﻿using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface INoteGroupsRepository
    {
        NoteGroup CreateNoteGroup(Guid noteId, Guid groupId, Token accessToken);
        Group GetGroupBy(Guid noteId);
        IEnumerable<Note> GetAllNoteBy(Guid groupId);
        IEnumerable<Note> GetAllNoteBy(Guid userId, string name);
    }
}
