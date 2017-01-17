using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Equin.ApplicationFramework;
using PnpUtilGui.Models;
using PnpUtilGui.Properties;
using PnpUtilGui.Utils;

namespace PnpUtilGui
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Driver> _bindingSource = new BindingList<Driver>();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_LoadAsync;
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            await UpdateDataGridViewAsync();
            driverGridView.DataSource = new BindingListView<Driver>(_bindingSource);

            for (var i = 0; i < driverGridView.ColumnCount; i++)
            {
                if (i == 0)
                {
                    driverGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                    continue;
                }

                driverGridView.Columns[i].ReadOnly = true;
                driverGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;

                if (i == driverGridView.ColumnCount - 1)
                {
                    driverGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

            driverGridView.Update();
            driverGridView.Refresh();
        }

        private async Task UpdateDataGridViewAsync()
        {
            _bindingSource.Clear();

            foreach (var driver in await PnpUtilHelper.EnumDrivers())
            {
                _bindingSource.Add(driver);
            }
        }

        private async void DeleteButton_ClickAsync(object sender, EventArgs e)
        {
            var selected = _bindingSource.Where(x => x.Checked).ToList();
            var result =
                MessageBox.Show(
                    string.Format(Resources.Form1_DeleteButton_Click_MessageBoxText, selected.Count),
                    Resources.Form1_DeleteButton_Click_MessageBox_Caption,
                    MessageBoxButtons.YesNo
                );

            if (result != DialogResult.Yes)
            {
                return;
            }

            foreach (var item in selected)
            {
                logTextBox.AppendText($"Driver: {item.FileName}{Environment.NewLine}");
                var output = await PnpUtilHelper.DeleteDriver(item.FileName, forceCheckBox.Checked);
                logTextBox.AppendText(output + Environment.NewLine);
                logTextBox.AppendText(Environment.NewLine);
            }

            logTextBox.AppendText(
                "==================================================" + Environment.NewLine + Environment.NewLine);

            await UpdateDataGridViewAsync();
        }

        private async void RefreshButton_ClickAsync(object sender, EventArgs e)
        {
            await UpdateDataGridViewAsync();
        }

        private async void ExportButton_ClickAsync(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                return;
            }

            foreach (var driver in _bindingSource.Where(x => x.Checked))
            {
                var output = await PnpUtilHelper.ExportDriver(driver.FileName, dialog.SelectedPath);
                logTextBox.AppendText(output + Environment.NewLine);
                logTextBox.AppendText(Environment.NewLine);
            }

            logTextBox.AppendText(
                "==================================================" + Environment.NewLine + Environment.NewLine);
        }

        private ListSortDirection _sortColumnDirection;

        private int _currentSortColumnIndex;

        private void driverGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (driverGridView.Columns[e.ColumnIndex].SortMode != DataGridViewColumnSortMode.NotSortable)
            {
                if (e.ColumnIndex == _currentSortColumnIndex)
                {
                    _sortColumnDirection = _sortColumnDirection == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                }

                _currentSortColumnIndex = e.ColumnIndex;

                switch (_sortColumnDirection)
                {
                    case ListSortDirection.Ascending:
                        driverGridView.Sort(
                            driverGridView.Columns[_currentSortColumnIndex],
                            ListSortDirection.Ascending);
                        break;
                    case ListSortDirection.Descending:
                        driverGridView.Sort(
                            driverGridView.Columns[_currentSortColumnIndex],
                            ListSortDirection.Descending);
                        break;
                }
            }
        }
    }
}