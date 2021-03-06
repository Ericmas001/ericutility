﻿using System;
using Com.Ericmas001.Wpf.Entities;
using Com.Ericmas001.Wpf.ViewModels.Tabs;

namespace Com.Ericmas001.Wpf.ViewModels.Sections
{
    public abstract class TabSection : NewTabViewModel
    {
        public event EventHandler OnAfterExpanded = delegate { };

        private bool m_IsExpanded;

        public bool IsExpanded
        {
            get { return m_IsExpanded; }
            set
            {
                if (m_IsExpanded != value)
                {
                    m_IsExpanded = value;
                    RaisePropertyChanged("IsExpanded");
                    if (m_IsExpanded)
                        OnAfterExpanded(this, new EventArgs());
                }
            }
        }

        public TabSectionInfo Info { get; protected set; }

        public virtual int SectionWidth { get { return 400; } }
    }
}
