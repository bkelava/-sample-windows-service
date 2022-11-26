using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWindowsService
{
    public partial class SampleWindowsService : ServiceBase
    {
        #region Fields

        private static bool _stateRunning = false;
        private static CancellationTokenSource _tokenSource;
        private Task t;
        private CancellationToken token;

        #endregion Fields

        #region Constructors

        public SampleWindowsService()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            if (!_stateRunning)
            {
                _stateRunning = true;
                _tokenSource = new CancellationTokenSource();

                token = _tokenSource.Token;
                token.ThrowIfCancellationRequested();

                try
                {
                    this.EventLog.WriteEntry($"Mono Sample Test Service - Starting - at {DateTime.Now}", EventLogEntryType.Information);

                    t = Task.Factory.StartNew(() => RunDelayAsync(token), token);
                    t.Wait();
                }
                catch (System.Exception ex)
                {
                    this.EventLog.WriteEntry($"Mono Sample Test Service - Caught exception when starting - at {DateTime.Now}. Details {ex}", EventLogEntryType.Information);
                    _tokenSource.Cancel();
                }
            }
        }

        protected override void OnStop()
        {
            if (_stateRunning)
            {
                try
                {
                    this.EventLog.WriteEntry($"Mono Sample Test Service - Shutting down - at {DateTime.Now}", EventLogEntryType.Information);
                    if (_tokenSource != null)
                    {
                        _tokenSource.Cancel();
                    }
                }
                finally
                {
                    if (_tokenSource != null)
                    {
                        _tokenSource.Dispose();
                    }
                    base.OnStop();
                }
            }
        }

        private async Task RunDelayAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(5000, token);
                this.EventLog.WriteEntry($"Mono Sample Test Service - Nothing to do, sleeping - at {DateTime.Now}", EventLogEntryType.Information);
            }
        }

        #endregion Methods
    }
}