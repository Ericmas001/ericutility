﻿using System.Collections.Generic;

namespace Com.Ericmas001.Net.JSON
{
    public class JsonObjectCollection : JsonCollection
    {
        public JsonObjectCollection()
        {
        }

        public JsonObjectCollection(string name)
            : base(name)
        {
        }

        public JsonObjectCollection(IEnumerable<JsonObject> collection)
            : base(collection)
        {
        }

        public JsonObjectCollection(string name, IEnumerable<JsonObject> collection)
            : base(name, collection)
        {
        }

        protected override char BeginCollection
        {
            get { return '{'; }
        }

        protected override char EndCollection
        {
            get { return '}'; }
        }

        public JsonObject this[string name]
        {
            get
            {
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].Name == name)
                    {
                        return this[i];
                    }
                }

                return null;
            }
        }
    }
}