using Com.Ericmas001.Downloader;
using Com.Ericmas001.Util;
using Com.Ericmas001.Util.IO;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace Com.Ericmas001.Windows.Forms.Downloader
{
    public partial class DownloadList : UserControl
    {
        private delegate void ActionDownloader(Com.Ericmas001.Downloader.Downloader d, ListViewItem item);

        private Hashtable mapItemToDownload = new Hashtable();
        private Hashtable mapDownloadToItem = new Hashtable();

        private ListViewItem lastSelection = null;

        public DownloadList()
        {
            InitializeComponent();

            DownloadManager.Instance.DownloadAdded += new EventHandler<DownloaderEventArgs>(Instance_DownloadAdded);
            DownloadManager.Instance.DownloadRemoved += new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);
            DownloadManager.Instance.DownloadEnded += new EventHandler<DownloaderEventArgs>(Instance_DownloadEnded);

            for (int i = 0; i < DownloadManager.Instance.Downloads.Count; i++)
            {
                AddDownload(DownloadManager.Instance.Downloads[i]);
            }

            lvwDownloads.SmallImageList = FileTypeImageList.GetSharedInstance();
        }

        private void DownloadList_Load(object sender, EventArgs e)
        {
            DownloadManager.Instance.BeginAddBatchDownloads += new EventHandler(Instance_BeginAddBatchDownloads);
            DownloadManager.Instance.EndAddBatchDownloads += new EventHandler(Instance_EndAddBatchDownloads);
        }

        private void Instance_EndAddBatchDownloads(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)lvwDownloads.EndUpdate);
        }

        private void Instance_BeginAddBatchDownloads(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)lvwDownloads.BeginUpdate);
        }

        public void ImportFromTextFile()
        {
            using (ImportFromFileForm importFile = new ImportFromFileForm())
            {
                if (importFile.ShowDialog() == DialogResult.OK)
                {
                    AddDownloadURLs(
                        importFile.URLs,
                        1,
                        importFile.DownloadPath,
                        0);
                }
            }
        }

        public void NewFileDownload(string url, bool modal)
        {
            if (modal)
            {
                using (NewDownloadForm newDownload = new NewDownloadForm())
                {
                    if (!String.IsNullOrEmpty(url))
                    {
                        newDownload.DownloadLocation = ResourceLocation.FromURL(url);
                    }

                    newDownload.ShowDialog();
                }
            }
            else
            {
                NewDownloadForm newDownload = new NewDownloadForm();

                if (!String.IsNullOrEmpty(url))
                {
                    newDownload.DownloadLocation = ResourceLocation.FromURL(url);
                }

                newDownload.ShowInTaskbar = true;
                newDownload.MinimizeBox = true;
                newDownload.Show();
                newDownload.Focus();
                newDownload.TopMost = true;
            }
        }

        public void StartSelections()
        {
            DownloadsAction(
                delegate(Com.Ericmas001.Downloader.Downloader d, ListViewItem item)
                {
                    d.Start();
                }
            );
        }

        public void Pause()
        {
            DownloadsAction(
                delegate(Com.Ericmas001.Downloader.Downloader d, ListViewItem item)
                {
                    d.Pause();
                }
            );
        }

        public void PauseAll()
        {
            DownloadManager.Instance.PauseAll();
            UpdateList();
        }

        public void RemoveSelections()
        {
            if (MessageBox.Show("Are you sure that you want to remove selected downloads?",
                this.ParentForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    DownloadManager.Instance.DownloadRemoved -= new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);

                    DownloadsAction(
                        delegate(Com.Ericmas001.Downloader.Downloader d, ListViewItem item)
                        {
                            lvwDownloads.Items.RemoveAt(item.Index);
                            DownloadManager.Instance.RemoveDownload(d);
                        }
                    );
                }
                finally
                {
                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);

                    DownloadManager.Instance.DownloadRemoved += new EventHandler<DownloaderEventArgs>(Instance_DownloadRemoved);
                }
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            string strRate = String.Format("{0:0.##} kbps", DownloadManager.Instance.TotalDownloadRate / 1024.0);
            UpdateList();
        }

        public void SelectAll()
        {
            using (DownloadManager.Instance.LockDownloadList(false))
            {
                lvwDownloads.BeginUpdate();
                try
                {
                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);

                    for (int i = 0; i < lvwDownloads.Items.Count; i++)
                    {
                        lvwDownloads.Items[i].Selected = true;
                    }
                }
                finally
                {
                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);

                    lvwDownloads.EndUpdate();
                }
            }
        }

        public void RemoveCompleted()
        {
            lvwDownloads.BeginUpdate();
            try
            {
                DownloadManager.Instance.ClearEnded();
                UpdateList();
            }
            finally
            {
                lvwDownloads.EndUpdate();
            }
        }

        public void AddDownload(string src, string dest, int nbSegments = 1)
        {
            tmrRefresh.Start();
            DownloadManager.Instance.Add(ResourceLocation.FromURL(src), null, dest, nbSegments, true);
        }

        public void AddDownloadURLs(
            ResourceLocation[] args,
            int segments,
            string path,
            int nrOfSubfolders)
        {
            tmrRefresh.Start();
            if (args == null) return;

            if (path == null)
            {
                path = "";
            }
            else
            {
                path = PathHelper.GetWithBackslash(path);
            }

            try
            {
                DownloadManager.Instance.OnBeginAddBatchDownloads();

                foreach (ResourceLocation rl in args)
                {
                    Uri uri = new Uri(rl.URL);

                    string fileName = uri.Segments[uri.Segments.Length - 1];
                    fileName = HttpUtility.UrlDecode(fileName).Replace("/", "\\");

                    DownloadManager.Instance.Add(
                        rl,
                        null,
                        path + fileName,
                        segments,
                        false);
                }
            }
            finally
            {
                DownloadManager.Instance.OnEndAddBatchDownloads();
            }
        }

        private void lvwDownloads_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateSegments();

            UpdateUI();
        }

        public void UpdateUI()
        {
            bool isSelected = lvwDownloads.SelectedItems.Count > 0;

            removeToolStripMenuItem.Enabled = isSelected;
            startToolStripMenuItem.Enabled = isSelected;
            pauseToolStripMenuItem.Enabled = isSelected;

            isSelected = lvwDownloads.SelectedItems.Count == 1;
            copyURLToClipboardToolStripMenuItem.Enabled = isSelected;

            showInExplorerToolStripMenuItem.Enabled = isSelected;
            openFileToolStripMenuItem.Enabled = isSelected;

            OnSelectionChange();
        }

        public event EventHandler SelectionChange;

        protected virtual void OnSelectionChange()
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, EventArgs.Empty);
            }
        }

        private void copyURLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SelectedDownloaders[0].ResourceLocation.URL);
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", String.Format("/select,{0}", SelectedDownloaders[0].LocalFile));
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(SelectedDownloaders[0].LocalFile);
            }
            catch (Exception)
            {
            }
        }

        public int SelectedCount
        {
            get
            {
                return lvwDownloads.SelectedItems.Count;
            }
        }

        public Com.Ericmas001.Downloader.Downloader[] SelectedDownloaders
        {
            get
            {
                if (lvwDownloads.SelectedItems.Count > 0)
                {
                    Com.Ericmas001.Downloader.Downloader[] downloaders = new Com.Ericmas001.Downloader.Downloader[lvwDownloads.SelectedItems.Count];
                    for (int i = 0; i < downloaders.Length; i++)
                    {
                        downloaders[i] = mapItemToDownload[lvwDownloads.SelectedItems[i]] as Com.Ericmas001.Downloader.Downloader;
                    }
                    return downloaders;
                }

                return null;
            }
        }

        private void Instance_DownloadRemoved(object sender, DownloaderEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
                {
                    ListViewItem item = mapDownloadToItem[e.Downloader] as ListViewItem;

                    if (item != null)
                    {
                        if (item.Selected)
                        {
                            lastSelection = null;

                            lvwSegments.Items.Clear();
                            //lvwDownloads.SelectedItems.Clear();
                        }

                        mapDownloadToItem[e.Downloader] = null;
                        mapItemToDownload[item] = null;

                        item.Remove();
                    }
                }
            );
        }

        private void Instance_DownloadAdded(object sender, DownloaderEventArgs e)
        {
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate() { AddDownload(e.Downloader); });
            }
            else
            {
                AddDownload(e.Downloader);
            }
        }

        private void AddDownload(Com.Ericmas001.Downloader.Downloader d)
        {
            tmrRefresh.Start();
            d.RestartingSegment += new EventHandler<SegmentEventArgs>(download_RestartingSegment);
            d.SegmentStoped += new EventHandler<SegmentEventArgs>(download_SegmentEnded);
            d.SegmentFailed += new EventHandler<SegmentEventArgs>(download_SegmentFailed);
            d.SegmentStarted += new EventHandler<SegmentEventArgs>(download_SegmentStarted);
            d.InfoReceived += new EventHandler(download_InfoReceived);
            d.SegmentStarting += new EventHandler<SegmentEventArgs>(Downloader_SegmentStarting);

            string ext = Path.GetExtension(d.LocalFile);

            ListViewItem item = new ListViewItem();

            item.ImageIndex = FileTypeImageList.GetImageIndexByExtention(ext);
            item.Text = Path.GetFileName(d.LocalFile);

            // size
            item.SubItems.Add(ByteFormatter.ToString(d.FileSize));
            // completed
            item.SubItems.Add(ByteFormatter.ToString(d.Transfered));
            // progress
            item.SubItems.Add(String.Format("{0:0.##}%", d.Progress));
            // left
            item.SubItems.Add(TimeSpanFormatter.ToString(d.Left));
            // rate
            item.SubItems.Add("0");
            // added
            item.SubItems.Add(d.CreatedDateTime.ToShortDateString() + " " + d.CreatedDateTime.ToShortTimeString());
            // state
            item.SubItems.Add(d.State.ToString());
            // resume
            item.SubItems.Add(GetResumeStr(d));
            // url
            item.SubItems.Add(d.ResourceLocation.URL);

            mapDownloadToItem[d] = item;
            mapItemToDownload[item] = d;

            lvwDownloads.Items.Add(item);
        }

        private static string GetResumeStr(Com.Ericmas001.Downloader.Downloader d)
        {
            return (d.RemoteFileInfo != null && d.RemoteFileInfo.AcceptRanges ? "Yes" : "No");
        }

        public void UpdateList()
        {
            int maxpercent = 0;
            int currentpercent = 0;
            for (int i = 0; i < lvwDownloads.Items.Count; i++)
            {
                ListViewItem item = lvwDownloads.Items[i];
                if (item == null) return;
                Com.Ericmas001.Downloader.Downloader d = mapItemToDownload[item] as Com.Ericmas001.Downloader.Downloader;
                if (d == null) return;

                DownloaderState state;

                if (item.Tag == null) state = DownloaderState.Working;
                else state = (DownloaderState)item.Tag;

                if (state != d.State ||
                    state == DownloaderState.Working ||
                    state == DownloaderState.WaitingForReconnect)
                {
                    maxpercent += 100;
                    currentpercent += (int)d.Progress;
                    item.SubItems[1].Text = ByteFormatter.ToString(d.FileSize);
                    item.SubItems[2].Text = ByteFormatter.ToString(d.Transfered);
                    item.SubItems[3].Text = String.Format("{0:0.##}%", d.Progress);
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(d.Left);
                    item.SubItems[5].Text = String.Format("{0:0.##}", d.Rate / 1024.0);

                    if (d.LastError != null)
                    {
                        item.SubItems[7].Text = d.State.ToString() + ", " + d.LastError.Message;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(d.StatusMessage))
                        {
                            item.SubItems[7].Text = d.State.ToString();
                        }
                        else
                        {
                            item.SubItems[7].Text = d.State.ToString() + ", " + d.StatusMessage;
                        }
                    }

                    item.SubItems[8].Text = GetResumeStr(d);
                    item.SubItems[9].Text = d.ResourceLocation.URL;
                    item.Tag = d.State;
                }
            }
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager tbManager = TaskbarManager.Instance;
                tbManager.SetProgressValue(currentpercent, maxpercent);
            }
            UpdateSegments();
            if (maxpercent == 0)
                tmrRefresh.Stop();
        }

        private void UpdateSegments()
        {
            try
            {
                lvwSegments.BeginUpdate();

                if (lvwDownloads.SelectedItems.Count == 1)
                {
                    ListViewItem newSelection = lvwDownloads.SelectedItems[0];
                    Com.Ericmas001.Downloader.Downloader d = mapItemToDownload[newSelection] as Com.Ericmas001.Downloader.Downloader;

                    if (lastSelection == newSelection)
                    {
                        if (d.Segments.Count == lvwSegments.Items.Count)
                        {
                            UpdateSegmentsWithoutInsert(d);
                        }
                        else
                        {
                            UpdateSegmentsInserting(d, newSelection);
                        }
                    }
                    else
                    {
                        UpdateSegmentsInserting(d, newSelection);
                    }
                }
                else
                {
                    lastSelection = null;

                    blockedProgressBar1.BlockList.Clear();
                    blockedProgressBar1.Refresh();

                    lvwSegments.Items.Clear();
                }
            }
            finally
            {
                lvwSegments.EndUpdate();
            }
        }

        private void DownloadsAction(ActionDownloader action)
        {
            if (lvwDownloads.SelectedItems.Count > 0)
            {
                try
                {
                    lvwDownloads.BeginUpdate();

                    lvwDownloads.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);

                    for (int i = lvwDownloads.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem item = lvwDownloads.SelectedItems[i];
                        action((Com.Ericmas001.Downloader.Downloader)mapItemToDownload[item], item);
                    }

                    lvwDownloads.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwDownloads_ItemSelectionChanged);
                    lvwDownloads_ItemSelectionChanged(null, null);
                }
                finally
                {
                    lvwDownloads.EndUpdate();
                    UpdateSegments();
                }
            }
        }

        private void UpdateSegmentsWithoutInsert(Com.Ericmas001.Downloader.Downloader d)
        {
            for (int i = 0; i < d.Segments.Count; i++)
            {
                lvwSegments.Items[i].SubItems[0].Text = d.Segments[i].CurrentTry.ToString();
                lvwSegments.Items[i].SubItems[1].Text = String.Format("{0:0.##}%", d.Segments[i].Progress);
                lvwSegments.Items[i].SubItems[2].Text = ByteFormatter.ToString(d.Segments[i].Transfered);
                lvwSegments.Items[i].SubItems[3].Text = ByteFormatter.ToString(d.Segments[i].TotalToTransfer);
                lvwSegments.Items[i].SubItems[4].Text = ByteFormatter.ToString(d.Segments[i].InitialStartPosition);
                lvwSegments.Items[i].SubItems[5].Text = ByteFormatter.ToString(d.Segments[i].EndPosition);
                lvwSegments.Items[i].SubItems[6].Text = String.Format("{0:0.##}", d.Segments[i].Rate / 1024.0);
                lvwSegments.Items[i].SubItems[7].Text = TimeSpanFormatter.ToString(d.Segments[i].Left);
                if (d.Segments[i].LastError != null)
                {
                    lvwSegments.Items[i].SubItems[8].Text = d.Segments[i].State.ToString() + ", " + d.Segments[i].LastError.Message;
                }
                else
                {
                    lvwSegments.Items[i].SubItems[8].Text = d.Segments[i].State.ToString();
                }
                lvwSegments.Items[i].SubItems[9].Text = d.Segments[i].CurrentURL;

                this.blockedProgressBar1.BlockList[i].BlockSize = d.Segments[i].TotalToTransfer;
                this.blockedProgressBar1.BlockList[i].PercentProgress = (float)d.Segments[i].Progress;
            }

            this.blockedProgressBar1.Refresh();
        }

        private void UpdateSegmentsInserting(Com.Ericmas001.Downloader.Downloader d, ListViewItem newSelection)
        {
            lastSelection = newSelection;

            lvwSegments.Items.Clear();

            List<Block> blocks = new List<Block>();

            for (int i = 0; i < d.Segments.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Text = d.Segments[i].CurrentTry.ToString();
                item.SubItems.Add(String.Format("{0:0.##}%", d.Segments[i].Progress));
                item.SubItems.Add(ByteFormatter.ToString(d.Segments[i].Transfered));
                item.SubItems.Add(ByteFormatter.ToString(d.Segments[i].TotalToTransfer));
                item.SubItems.Add(ByteFormatter.ToString(d.Segments[i].InitialStartPosition));
                item.SubItems.Add(ByteFormatter.ToString(d.Segments[i].EndPosition));
                item.SubItems.Add(String.Format("{0:0.##}", d.Segments[i].Rate / 1024.0));
                item.SubItems.Add(TimeSpanFormatter.ToString(d.Segments[i].Left));
                item.SubItems.Add(d.Segments[i].State.ToString());
                item.SubItems.Add(d.Segments[i].CurrentURL);

                lvwSegments.Items.Add(item);

                blocks.Add(new Block(d.Segments[i].TotalToTransfer, (float)d.Segments[i].Progress));
            }

            this.blockedProgressBar1.BlockList = blocks;
        }

        private void download_InfoReceived(object sender, EventArgs e)
        {
            Com.Ericmas001.Downloader.Downloader d = (Com.Ericmas001.Downloader.Downloader)sender;

            Log(
                d,
                String.Format(
                "Connected to: {2}. File size = {0}, Resume = {1}",
                ByteFormatter.ToString(d.FileSize),
                d.RemoteFileInfo.AcceptRanges,
                d.ResourceLocation.URL),
                LogMode.Information);
        }

        private void Downloader_SegmentStarting(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Starting segment for {3}, start position = {0}, end position {1}, segment size = {2}",
                ByteFormatter.ToString(e.Segment.InitialStartPosition),
                ByteFormatter.ToString(e.Segment.EndPosition),
                ByteFormatter.ToString(e.Segment.TotalToTransfer),
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        private void download_SegmentStarted(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Started segment for {3}, start position = {0}, end position {1}, segment size = {2}",
                ByteFormatter.ToString(e.Segment.InitialStartPosition),
                ByteFormatter.ToString(e.Segment.EndPosition),
                ByteFormatter.ToString(e.Segment.TotalToTransfer),
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        private void download_SegmentFailed(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) failed for {2}, reason = {1}",
                e.Segment.Index,
                e.Segment.LastError.Message,
                e.Downloader.ResourceLocation.URL),
                LogMode.Error);
        }

        private void download_SegmentEnded(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) ended for {1}",
                e.Segment.Index,
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        private void download_RestartingSegment(object sender, SegmentEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download segment ({0}) is restarting for {1}",
                e.Segment.Index,
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        private void Instance_DownloadEnded(object sender, DownloaderEventArgs e)
        {
            Log(
                e.Downloader,
                String.Format(
                "Download ended {0}",
                e.Downloader.ResourceLocation.URL),
                LogMode.Information);
        }

        private enum LogMode
        {
            Error,
            Information
        }

        private void Log(Com.Ericmas001.Downloader.Downloader downloader, string msg, LogMode m)
        {
            try
            {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate()
                  {
                      int len = richLog.Text.Length;
                      if (len > 0)
                      {
                          richLog.SelectionStart = len;
                      }

                      if (m == LogMode.Error)
                      {
                          richLog.SelectionColor = Color.Red;
                      }
                      else
                      {
                          richLog.SelectionColor = Color.Blue;
                      }

                      richLog.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch { }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richLog.Clear();
        }

        private void newDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFileDownload(null, true);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSelections();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelections();
        }

        private void SwapElements(int idx)
        {
            ListViewItem it1 = lvwDownloads.Items[idx];
            ListViewItem it2 = lvwDownloads.Items[idx - 1];

            lvwDownloads.Items.RemoveAt(idx);
            lvwDownloads.Items.RemoveAt(idx - 1);

            lvwDownloads.Items.Insert(idx - 1, it1);
            lvwDownloads.Items.Insert(idx, it2);

            DownloadManager.Instance.SwapDownloads(idx, true);
        }

        public void MoveSelectionsDown()
        {
            int selCount = lvwDownloads.SelectedItems.Count;
            if (selCount > 0)
            {
                try
                {
                    lvwDownloads.BeginUpdate();

                    ListViewItem lvi = null;

                    using (DownloadManager.Instance.LockDownloadList(true))
                    {
                        for (int i = selCount - 1; i >= 0; i--)
                        {
                            lvi = lvwDownloads.SelectedItems[i];

                            if (lvi.Index < lvwDownloads.Items.Count - 1
                                && !lvwDownloads.Items[lvi.Index + 1].Selected)
                            {
                                SwapElements(lvi.Index + 1);
                            }
                        }
                    }

                    lvwDownloads.SelectedItems[selCount - 1].EnsureVisible();
                }
                finally
                {
                    lvwDownloads.EndUpdate();
                }
            }
        }

        public void MoveSelectionsUp()
        {
            int selCount = lvwDownloads.SelectedItems.Count;

            if (selCount > 0)
            {
                try
                {
                    lvwDownloads.BeginUpdate();

                    ListViewItem lvi = null;

                    using (DownloadManager.Instance.LockDownloadList(true))
                    {
                        for (int i = 0; i < selCount; i++)
                        {
                            lvi = lvwDownloads.SelectedItems[i];

                            if (lvi.Index > 0
                                && !lvwDownloads.Items[lvi.Index - 1].Selected)
                            {
                                SwapElements(lvi.Index);
                            }
                        }
                    }

                    lvwDownloads.SelectedItems[0].EnsureVisible();
                }
                finally
                {
                    lvwDownloads.EndUpdate();
                }
            }
        }

        private void popUpContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateUI();
        }

        private void lvwDownloads_DoubleClick(object sender, EventArgs e)
        {
            UpdateUI();

            openFileToolStripMenuItem_Click(sender, e);
        }
    }
}