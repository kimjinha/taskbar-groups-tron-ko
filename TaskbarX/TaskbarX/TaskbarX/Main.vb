Option Strict On


Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading


Public Class Main

    Public Shared noty As New NotifyIcon

    Public Shared Sub Main()
        Try


            'Set default settings
            Settings.TaskbarStyle = 0
            Settings.PrimaryTaskbarOffset = 0
            Settings.SecondaryTaskbarOffset = 0
            Settings.CenterPrimaryOnly = 0
            Settings.CenterSecondaryOnly = 0
            Settings.AnimationStyle = "cubiceaseinout"
            Settings.AnimationSpeed = 300
            Settings.LoopRefreshRate = 400
            Settings.CenterInBetween = 0
            Settings.DontCenterTaskbar = 0
            Settings.FixToolbarsOnTrayChange = 1
            Settings.OnBatteryAnimationStyle = "cubiceaseinout"
            Settings.OnBatteryLoopRefreshRate = 400
            Settings.RevertZeroBeyondTray = 1
            Settings.TaskbarRounding = 0
            Settings.TaskbarSegments = 0

            Dim stopgiven As Boolean = False

            'Read the arguments for the settings
            Dim arguments() As String = Environment.GetCommandLineArgs
            For Each argument In arguments
                Dim val() As String = Split(argument, "=")
                If argument.Contains("-stop") Then
                    stopgiven = True
                End If
                If argument.Contains("-showstartmenu") Then
                    Win32.ShowStartMenu()
                    End
                End If
                If argument.Contains("-console=") Then
                    Win32.AllocConsole()
                    Settings.ConsoleEnabled = 1
                End If
                If argument.Contains("-tbs=") Then
                    Settings.TaskbarStyle = CInt(val(1))
                End If

                If argument.Contains("-color=") Then
                    Dim colorval As String = val(1)
                    Dim colorsep = colorval.Split(CType(";", Char()))

                    Settings.TaskbarStyleRed = CInt(colorsep(0))
                    Settings.TaskbarStyleGreen = CInt(colorsep(1))
                    Settings.TaskbarStyleBlue = CInt(colorsep(2))
                    Settings.TaskbarStyleAlpha = CInt(colorsep(3))
                End If

                If argument.Contains("-ptbo=") Then
                    Settings.PrimaryTaskbarOffset = CInt(val(1))
                End If
                If argument.Contains("-stbo=") Then
                    Settings.SecondaryTaskbarOffset = CInt(val(1))
                End If
                If argument.Contains("-cpo=") Then
                    Settings.CenterPrimaryOnly = CInt(val(1))
                End If
                If argument.Contains("-cso=") Then
                    Settings.CenterSecondaryOnly = CInt(val(1))
                End If
                If argument.Contains("-as=") Then
                    Settings.AnimationStyle = CType(val(1), String)
                End If
                If argument.Contains("-asp=") Then
                    Settings.AnimationSpeed = CInt(val(1))
                End If
                If argument.Contains("-lr=") Then
                    Settings.LoopRefreshRate = CInt(val(1))
                End If
                If argument.Contains("-cib=") Then
                    Settings.CenterInBetween = CInt(val(1))
                End If
                If argument.Contains("-obas=") Then
                    Settings.OnBatteryAnimationStyle = CType(val(1), String)
                End If
                If argument.Contains("-oblr=") Then
                    Settings.OnBatteryLoopRefreshRate = CInt(val(1))
                End If
                If argument.Contains("-ftotc=") Then
                    Settings.FixToolbarsOnTrayChange = CInt(val(1))
                End If
                If argument.Contains("-rzbt=") Then
                    Settings.RevertZeroBeyondTray = CInt(val(1))
                End If
                If argument.Contains("-sr=") Then
                    Settings.SkipResolution = CInt(val(1))
                End If
                If argument.Contains("-sr2=") Then
                    Settings.SkipResolution2 = CInt(val(1))
                End If
                If argument.Contains("-sr3=") Then
                    Settings.SkipResolution3 = CInt(val(1))
                End If
                If argument.Contains("-dtbsowm=") Then
                    Settings.DefaultTaskbarStyleOnWinMax = CInt(val(1))
                End If
                If argument.Contains("-cfsa=") Then
                    Settings.CheckFullscreenApp = CInt(val(1))
                End If
                If argument.Contains("-dct=") Then
                    Settings.DontCenterTaskbar = CInt(val(1))
                End If
                If argument.Contains("-hps=") Then
                    Settings.HidePrimaryStartButton = CInt(val(1))
                End If
                If argument.Contains("-hss=") Then
                    Settings.HideSecondaryStartButton = CInt(val(1))
                End If
                If argument.Contains("-hpt=") Then
                    Settings.HidePrimaryNotifyWnd = CInt(val(1))
                End If
                If argument.Contains("-hst=") Then
                    Settings.HideSecondaryNotifyWnd = CInt(val(1))
                End If
                If argument.Contains("-sti=") Then
                    ''Settings.ShowTrayIcon = CInt(val(1))
                    Settings.ShowTrayIcon = CInt(0)
                End If
                If argument.Contains("-tbsom=") Then
                    Settings.TaskbarStyleOnMax = CInt(val(1))
                End If
                If argument.Contains("-stsb=") Then
                    Settings.StickyStartButton = CInt(val(1))
                End If
                If argument.Contains("-tpop=") Then
                    Settings.TotalPrimaryOpacity = CInt(val(1))
                End If
                If argument.Contains("-tsop=") Then
                    Settings.TotalSecondaryOpacity = CInt(val(1))
                End If
                If argument.Contains("-tbr=") Then
                    Settings.TaskbarRounding = CInt(val(1))
                End If
                If argument.Contains("-tbsg=") Then
                    Settings.TaskbarSegments = CInt(val(1))
                End If
                If argument.Contains("-tronline=") Then
                    '-tronline=0 off, 1 light back-and-forth, 2 steady neon, 3 light cycle (bike + wall)
                    Integer.TryParse(val(1), TronLine.Style)
                    TronLine.Enabled = TronLine.Style > 0
                End If
                If argument.Contains("-troncolor=") Then 'R;G;B
                    Dim c = val(1).Split(";"c)
                    If c.Length = 3 Then TronLine.LineColor = Color.FromArgb(CInt(c(0)), CInt(c(1)), CInt(c(2)))
                End If
                If argument.Contains("-tronspeed=") Then 'animation speed 1 (slow) .. 10 (fast), 5 = default
                    Integer.TryParse(val(1), TronLine.Speed)
                    TronLine.Speed = Math.Max(1, Math.Min(10, TronLine.Speed))
                End If
                If argument.Contains("-tronglow=") Then 'neon bloom, 0..20 px at 100 %
                    Integer.TryParse(val(1), TronLine.Glow)
                    TronLine.Glow = Math.Max(0, Math.Min(20, TronLine.Glow))
                End If
            Next

            'Kill every other running instance of TaskbarX
            Try
                For Each prog As Process In Process.GetProcessesByName("TaskbarX")
                    If Not prog.Id = Process.GetCurrentProcess.Id Then
                        prog.Kill()
                    End If
                Next
            Catch
            End Try

            'If animation speed is lower than 1 then make it 1. Otherwise it will give an error.
            If Settings.AnimationSpeed <= 1 Then
                Settings.AnimationSpeed = 1
            End If

            'Makes the animations run smoother
            Dim currentProcess As Process = Process.GetCurrentProcess
            currentProcess.PriorityClass = ProcessPriorityClass.Idle

            'Prevent wrong position calculations
            Win32.SetProcessDpiAwareness(Win32.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware)

            'Wait for Shell_TrayWnd
            Dim Handle As IntPtr
            Do
                Console.WriteLine("Waiting for Shell_TrayWnd")
                Handle = Nothing
                Thread.Sleep(250)
                Dim Shell_TrayWnd = Win32.FindWindowByClass("Shell_TrayWnd", CType(0, IntPtr))
                Dim TrayNotifyWnd = Win32.FindWindowEx(Shell_TrayWnd, CType(0, IntPtr), "TrayNotifyWnd", Nothing)
                Dim ReBarWindow32 = Win32.FindWindowEx(Shell_TrayWnd, CType(0, IntPtr), "ReBarWindow32", Nothing)
                Dim MSTaskSwWClass = Win32.FindWindowEx(ReBarWindow32, CType(0, IntPtr), "MSTaskSwWClass", Nothing)
                Dim MSTaskListWClass = Win32.FindWindowEx(MSTaskSwWClass, CType(0, IntPtr), "MSTaskListWClass", Nothing)
                Handle = MSTaskListWClass
                'Lock the Taskbar
                '  Win32.PostMessage(Shell_TrayWnd, CUInt(&H111), CType(424, IntPtr), CType(vbNullString, IntPtr))
            Loop Until Not Handle = Nothing


            Dim Win11Taskbar = Win32.FindWindowEx(Win32.FindWindowByClass("Shell_TrayWnd", CType(0, IntPtr)), CType(0, IntPtr), "Windows.UI.Composition.DesktopWindowContentBridge", Nothing)
            If Not Win11Taskbar = CType(0, IntPtr) Then
                'Windows 11 Taskbar present
                Settings.DontCenterTaskbar = CInt(Val(1))
            End If


            If stopgiven = True Then
                noty.Visible = False
                TaskbarCenter.RevertToZero()
                ResetTaskbarStyle()
                End
            End If



            If Settings.ShowTrayIcon = 1 Then
                TrayIconBuster.TrayIconBuster.RemovePhantomIcons()
            End If

            'Just empty startup memory before starting
            ClearMemory()

            'Reset the taskbar style...
            ResetTaskbarStyle()








            If Settings.ShowTrayIcon = 1 Then


                noty.Text = "TaskbarX (L = Restart) (M = Config) (R = Stop)"
                noty.Icon = My.Resources.icon
                noty.Visible = True


            End If

            AddHandler noty.MouseClick, AddressOf MnuRef_Click



            'Start the TaskbarCenterer
            If Not Settings.DontCenterTaskbar = 1 Then
                Dim t1 As Thread = New Thread(AddressOf TaskbarCenter.TaskbarCenterer)
                t1.Start()
            End If

            'Start the TaskbarStyler if enabled
            If Settings.TaskbarStyle = 1 Or Settings.TaskbarStyle = 2 Or Settings.TaskbarStyle = 3 Or Settings.TaskbarStyle = 4 Or Settings.TaskbarStyle = 5 Then
                Dim t2 As Thread = New Thread(AddressOf TaskbarStyle.TaskbarStyler)
                t2.Start()
            End If

            'TRON neon line on the taskbar inner edge
            If TronLine.Enabled Then
                TronLine.Start()
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub




    Public Shared Sub Toaster(ByVal message As String)
        noty.BalloonTipTitle = "TaskbarX"
        noty.BalloonTipText = message
        noty.Visible = True
        noty.ShowBalloonTip(3000)
    End Sub

    Public Shared Sub MnuRef_Click(sender As Object, e As MouseEventArgs)

        If (e.Button = MouseButtons.Left) Then
            noty.Visible = False
            Application.Restart()
        ElseIf (e.Button = MouseButtons.Right) Then
            noty.Visible = False
            TaskbarCenter.RevertToZero()
            ResetTaskbarStyle()
            End
        ElseIf (e.Button = MouseButtons.Middle) Then
            If System.AppDomain.CurrentDomain.BaseDirectory.Contains("40210ChrisAndriessen") Then
                Try
                    Dim processInfo As ProcessStartInfo = New ProcessStartInfo With {
                        .WindowStyle = ProcessWindowStyle.Hidden,
                        .FileName = "cmd.exe",
                        .Arguments = " /c start shell:AppsFolder\40210ChrisAndriessen.FalconX_y1dazs5f5wq00!TaskbarXGUI"
                    }
                    Process.Start(processInfo)
                Catch
                End Try
            Else
                Try
                    System.Diagnostics.Process.Start("TaskbarX Configurator.exe")
                Catch
                End Try
            End If
        End If

    End Sub

#Region "Commands"

    Public Declare Function EnumWindows Lib "user32" (ByVal Adress As CallBack, ByVal y As Integer) As Integer

    Public Delegate Function CallBack(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean

    Public Shared ActiveWindows As New System.Collections.ObjectModel.Collection(Of IntPtr)

    Public Shared Function GetActiveWindows() As ObjectModel.Collection(Of IntPtr)
        windowHandles.Clear()
        EnumWindows(AddressOf Enumerator, 0)

        Dim maintaskbarfound As Boolean = False
        Dim sectaskbarfound As Boolean = False

        For Each Taskbar In windowHandles
            Dim sClassName As New StringBuilder("", 256)
            Call Win32.GetClassName(CType(Taskbar, IntPtr), sClassName, 256)
            If sClassName.ToString = "Shell_TrayWnd" Then
                maintaskbarfound = True
            End If
            If sClassName.ToString = "Shell_SecondaryTrayWnd" Then
                sectaskbarfound = True
            End If
            Console.WriteLine("=" & maintaskbarfound)
        Next

        If maintaskbarfound = False Then
            Try
                windowHandles.Add(Win32.FindWindow("Shell_TrayWnd", Nothing))
            Catch
            End Try
        End If

        If sectaskbarfound = False Then
            If Screen.AllScreens.Count >= 2 Then
                ''MsgBox(Screen.AllScreens.Count)
                Try
                    windowHandles.Add(Win32.FindWindow("Shell_SecondaryTrayWnd", Nothing))
                Catch
                End Try
            End If
        End If


        Return ActiveWindows
    End Function

    Public Shared Function Enumerator(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean
        Dim sClassName As New StringBuilder("", 256)
        Call Win32.GetClassName(hwnd, sClassName, 256)
        If sClassName.ToString = "Shell_TrayWnd" Or sClassName.ToString = "Shell_SecondaryTrayWnd" Then
            windowHandles.Add(hwnd)
        End If
        Return True
    End Function

    Public Shared windowHandles As ArrayList = New ArrayList()

    Public Shared Sub ResetTaskbarStyle()



        GetActiveWindows()



        Dim trays As New ArrayList
        For Each trayWnd As IntPtr In windowHandles
            ''Console.WriteLine(trayWnd)
            trays.Add(trayWnd)
        Next

        For Each tray As IntPtr In trays
            Dim trayptr As IntPtr = tray


            Win32.SendMessage(trayptr, Win32.WM_THEMECHANGED, True, 0)
            Win32.SendMessage(trayptr, Win32.WM_DWMCOLORIZATIONCOLORCHANGED, True, 0)
            Win32.SendMessage(trayptr, Win32.WM_DWMCOMPOSITIONCHANGED, True, 0)



            Dim tt As New Win32.RECT
            Win32.GetClientRect(trayptr, tt)



            ''Win32.SetWindowRgn(CType(trayptr, IntPtr), Win32.CreateRoundRectRgn(-1, -1, tt.Right + 1, tt.Bottom - tt.Top + 1, -1, -1), True)
            Win32.SetWindowRgn(CType(trayptr, IntPtr), Win32.CreateRectRgn(tt.Left, tt.Top, tt.Right, tt.Bottom), True)







        Next


    End Sub

    Public Shared Sub RestartExplorer()
        For Each MyProcess In Process.GetProcessesByName("explorer")
            MyProcess.Kill()
        Next
    End Sub

    Public Shared Function ClearMemory() As Int32
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
        Return Win32.SetProcessWorkingSetSize(Diagnostics.Process.GetCurrentProcess.Handle, -1, -1)
    End Function








#End Region

End Class

'TRON neon line: a 2px click-through topmost window on the inner edge of every taskbar.
'Runs its own STA message loop; dies with the TaskbarX process.
Public Class TronLine
    Public Shared Enabled As Boolean
    Public Shared Style As Integer = 1
    Public Shared LineColor As Color = Color.FromArgb(0, 229, 255) '#00E5FF
    Public Shared Glow As Integer = 6
    Public Shared Speed As Integer = 5 '1..10; each step ~1.3x faster, 1 = 1/3 speed, 10 = 4x
    'height of the bike strip next to the line (16 px at 100%)
    Private Shared ReadOnly BikeH As Integer = CInt(16 * Graphics.FromHwnd(IntPtr.Zero).DpiX / 96)

    Private Const Thickness As Integer = 2
    Private Shared ReadOnly HWND_TOPMOST As New IntPtr(-1)
    Private Const SWP_NOSIZE As UInteger = &H1UI
    Private Const SWP_NOMOVE As UInteger = &H2UI
    Private Const SWP_NOZORDER As UInteger = &H4UI
    Private Const SWP_NOACTIVATE As UInteger = &H10UI
    Private Const SWP_SHOWWINDOW As UInteger = &H40UI
    Private Const SWP_HIDEWINDOW As UInteger = &H80UI

    <DllImport("user32.dll")>
    Private Shared Function IsWindowVisible(hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    'taskbar hwnd -> line window (only touched on the line thread)
    Private Shared ReadOnly lines As New Dictionary(Of IntPtr, LineForm)

    Public Shared Sub Start()
        Dim t As New Thread(AddressOf Run)
        t.SetApartmentState(ApartmentState.STA)
        t.Start()
    End Sub

    Private Shared Sub Run()
        Try
            Dim timer As New System.Windows.Forms.Timer With {.Interval = 500}
            AddHandler timer.Tick, AddressOf Tick
            timer.Start()
            Tick(Nothing, EventArgs.Empty)
            Application.Run()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Shared Sub Tick(sender As Object, e As EventArgs)
        Try
            Dim bars As New List(Of IntPtr)
            Dim primary = Win32.FindWindow("Shell_TrayWnd", Nothing)
            If primary <> IntPtr.Zero Then bars.Add(primary)
            Dim sec = IntPtr.Zero
            Do
                sec = Win32.FindWindowEx(IntPtr.Zero, sec, "Shell_SecondaryTrayWnd", Nothing)
                If sec = IntPtr.Zero Then Exit Do
                bars.Add(sec)
            Loop

            'taskbar gone (explorer restart, monitor unplugged): drop its line
            For Each k In lines.Keys.ToList()
                If Not bars.Contains(k) Then
                    lines(k).Dispose()
                    lines.Remove(k)
                End If
            Next

            Dim fsMonitor = FullscreenMonitor()

            For Each bar In bars
                Dim f As LineForm = Nothing
                If Not lines.TryGetValue(bar, f) Then
                    f = New LineForm()
                    lines(bar) = f
                End If
                Dim r = LineRect(bar, fsMonitor)
                If r.IsEmpty Then
                    Win32.SetWindowPos(f.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_HIDEWINDOW Or SWP_NOACTIVATE Or SWP_NOMOVE Or SWP_NOSIZE Or SWP_NOZORDER)
                Else
                    'window = line + bloom strip on both sides (+ a bike strip on the outer side for the light-cycle style);
                    'everything but the drawing is per-pixel transparent and click-through
                    Dim gp = CInt(Glow * BikeH / 16) 'bloom in real pixels (BikeH/16 = DPI scale)
                    f.GlowPx = gp
                    f.BikeArea = 0
                    If r.Width >= r.Height Then
                        Dim br As Win32.RECT
                        Win32.GetWindowRect(bar, br)
                        f.OuterUp = (r.Top = br.Top) 'bottom taskbar: line on its top edge, outside = above
                        Dim outer = gp
                        If Style = 3 Then
                            f.BikeArea = BikeH
                            outer = Math.Max(gp, BikeH)
                        End If
                        Dim up = If(f.OuterUp, outer, gp), down = If(f.OuterUp, gp, outer)
                        f.Lane = up
                        r = New Rectangle(r.X, r.Y - up, r.Width, r.Height + up + down)
                    Else
                        f.Lane = gp
                        r = New Rectangle(r.X - gp, r.Y, r.Width + 2 * gp, r.Height)
                    End If
                    'reassert topmost every tick: clicking the taskbar raises it above us
                    Win32.SetWindowPos(f.Handle, HWND_TOPMOST, r.X, r.Y, r.Width, r.Height, SWP_SHOWWINDOW Or SWP_NOACTIVATE)
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    'Line on the inner edge of the visible part of the taskbar; Empty = hide.
    Private Shared Function LineRect(bar As IntPtr, fsMonitor As Rectangle) As Rectangle
        If Not IsWindowVisible(bar) Then Return Rectangle.Empty
        Dim wr As Win32.RECT
        If Not Win32.GetWindowRect(bar, wr) Then Return Rectangle.Empty
        Dim r = Rectangle.FromLTRB(wr.Left, wr.Top, wr.Right, wr.Bottom)
        Dim mon = Screen.FromRectangle(r).Bounds
        If mon = fsMonitor Then Return Rectangle.Empty
        Dim v = Rectangle.Intersect(r, mon)
        If Math.Min(v.Width, v.Height) <= 4 Then Return Rectangle.Empty 'auto-hidden (only a sliver on screen)

        If v.Width >= v.Height Then
            If r.Top + r.Height \ 2 > mon.Top + mon.Height \ 2 Then
                Return New Rectangle(v.Left, v.Top, v.Width, Thickness)              'bottom taskbar -> top edge
            End If
            Return New Rectangle(v.Left, v.Bottom - Thickness, v.Width, Thickness)   'top taskbar -> bottom edge
        End If
        If r.Left + r.Width \ 2 > mon.Left + mon.Width \ 2 Then
            Return New Rectangle(v.Left, v.Top, Thickness, v.Height)                 'right taskbar -> left edge
        End If
        Return New Rectangle(v.Right - Thickness, v.Top, Thickness, v.Height)        'left taskbar -> right edge
    End Function

    'Bounds of the monitor covered by a fullscreen foreground window (game, video); Empty if none.
    Private Shared Function FullscreenMonitor() As Rectangle
        Dim fg = Win32.GetForegroundWindow()
        If fg = IntPtr.Zero Then Return Rectangle.Empty
        Dim cls As New StringBuilder(256)
        Win32.GetClassName(fg, cls, 256)
        Select Case cls.ToString()
            Case "Progman", "WorkerW", "Shell_TrayWnd", "Shell_SecondaryTrayWnd"
                Return Rectangle.Empty
        End Select
        Dim wr As Win32.RECT
        If Not Win32.GetWindowRect(fg, wr) Then Return Rectangle.Empty
        Dim mon = Screen.FromHandle(fg).Bounds
        If Rectangle.FromLTRB(wr.Left, wr.Top, wr.Right, wr.Bottom).Contains(mon) Then Return mon
        Return Rectangle.Empty
    End Function

    'One per taskbar. Drawn into a 32-bit bitmap and pushed with UpdateLayeredWindow (per-pixel alpha),
    'so the neon can bloom softly into whatever is behind it while staying click-through.
    Private Class LineForm
        Inherits Form

        Public BikeArea As Integer   '0 = no bike strip
        Public OuterUp As Boolean    'horizontal: the outside of the taskbar is above the line
        Public Lane As Integer       'offset of the line from the window's top (horizontal) / left (vertical)
        Public GlowPx As Integer     'bloom width in pixels

        Private ReadOnly anim As New System.Windows.Forms.Timer With {.Interval = 15} '~60 fps
        Private ReadOnly clock As Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew()
        Private buf As Bitmap

        'Without this the timer snaps to the 15.6 ms system tick -> uneven frames (stutter)
        <DllImport("winmm.dll")>
        Private Shared Function timeBeginPeriod(uPeriod As UInteger) As UInteger
        End Function

        Shared Sub New()
            timeBeginPeriod(1)
        End Sub

        Public Sub New()
            FormBorderStyle = FormBorderStyle.None
            ShowInTaskbar = False
            StartPosition = FormStartPosition.Manual
            'Shown via SetWindowPos(SWP_SHOWWINDOW), which WinForms' Visible never sees -> ask Windows directly
            AddHandler anim.Tick, Sub(s, e)
                                      If IsHandleCreated AndAlso IsWindowVisible(Handle) Then Render()
                                  End Sub
            anim.Start()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                anim.Dispose()
                If buf IsNot Nothing Then buf.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim cp = MyBase.CreateParams
                'WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW | WS_EX_LAYERED | WS_EX_NOACTIVATE
                cp.ExStyle = cp.ExStyle Or &H8 Or &H20 Or &H80 Or &H80000 Or &H8000000
                Return cp
            End Get
        End Property

        '---------- colours (derived from the chosen line colour) ----------
        Private Shared Function Mix(a As Color, b As Color, k As Double) As Color
            Return Color.FromArgb(CInt(a.R + (b.R - a.R) * k), CInt(a.G + (b.G - a.G) * k), CInt(a.B + (b.B - a.B) * k))
        End Function
        Private Shared ReadOnly Property Neon As Color
            Get
                Return LineColor
            End Get
        End Property
        Private Shared ReadOnly Property Rail As Color   'dim rail
            Get
                Return Mix(Color.Black, LineColor, 0.4)
            End Get
        End Property
        Private Shared ReadOnly Property Core As Color   'white-hot
            Get
                Return Mix(LineColor, Color.White, 0.75)
            End Get
        End Property

        '---------- frame ----------
        Private Sub Render()
            Dim w = Width, h = Height
            If w <= 0 OrElse h <= 0 Then Return
            If buf Is Nothing OrElse buf.Width <> w OrElse buf.Height <> h Then
                If buf IsNot Nothing Then buf.Dispose()
                buf = New Bitmap(w, h, Imaging.PixelFormat.Format32bppPArgb)
            End If
            Using g = Graphics.FromImage(buf)
                g.Clear(Color.Transparent)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                Dim horiz = w >= h
                Dim lineRect = If(horiz, New RectangleF(0, Lane, w, Thickness), New RectangleF(Lane, 0, Thickness, h))
                Dim len = If(horiz, w, h)
                Dim t = clock.Elapsed.TotalSeconds * Math.Pow(2, (Speed - 5) / 2.5) 'speed scales animation time

                If Style = 3 AndAlso BikeArea > 0 Then
                    DrawLightCycle(g, lineRect, len, t)
                ElseIf Style = 2 Then
                    Lit(g, lineRect, horiz, Neon, Neon, True) 'steady neon
                Else
                    'one light, back and forth (3.5 s each way)
                    Lit(g, lineRect, horiz, Rail, Rail, False)
                    Dim phase = (t / 3.5) Mod 2.0
                    DrawRunner(g, lineRect, horiz, len, phase Mod 1.0, phase >= 1.0)
                End If
            End Using
            Present(buf)
        End Sub

        'A lit segment: soft bloom layers (wider, fainter) under a crisp core. a -> b is a gradient along the line.
        Private Sub Lit(g As Graphics, seg As RectangleF, horiz As Boolean, a As Color, b As Color, glow As Boolean)
            If seg.Width <= 0 OrElse seg.Height <= 0 Then Return
            If glow AndAlso GlowPx > 0 Then
                For i = GlowPx To 1 Step -1
                    Dim k = 1.0 - i / (GlowPx + 1.0)
                    Dim alpha = Math.Min(255, CInt(220.0 * k * k / GlowPx) + 3)
                    Dim r = If(horiz, RectangleF.FromLTRB(seg.Left, seg.Top - i, seg.Right, seg.Bottom + i),
                                      RectangleF.FromLTRB(seg.Left - i, seg.Top, seg.Right + i, seg.Bottom))
                    Using br = MakeBrush(r, Color.FromArgb(alpha, a), Color.FromArgb(alpha, b), horiz)
                        g.FillRectangle(br, r)
                    End Using
                Next
            End If
            Using br = MakeBrush(seg, a, b, horiz)
                g.FillRectangle(br, seg)
            End Using
        End Sub

        Private Shared Function MakeBrush(r As RectangleF, a As Color, b As Color, horiz As Boolean) As Brush
            If a = b Then Return New SolidBrush(a)
            'gradient box 1px larger along the line so the edges don't wrap around
            Dim gr = If(horiz, New RectangleF(r.X - 1, r.Y, r.Width + 2, Math.Max(1, r.Height)), New RectangleF(r.X, r.Y - 1, Math.Max(1, r.Width), r.Height + 2))
            Return New Drawing2D.LinearGradientBrush(gr, a, b, If(horiz, 0.0F, 90.0F))
        End Function

        'Part of the line between `start` and `start + length` (along the line)
        Private Shared Function Part(line As RectangleF, horiz As Boolean, start As Single, length As Single) As RectangleF
            Return If(horiz, New RectangleF(line.X + start, line.Y, length, line.Height), New RectangleF(line.X, line.Y + start, line.Width, length))
        End Function

        'Style 1: a light with a fading trail
        Private Sub DrawRunner(g As Graphics, line As RectangleF, horiz As Boolean, len As Integer, p As Double, reverse As Boolean)
            Dim trail = CSng(Math.Max(160, len \ 5))
            Dim headLen = 24.0F
            Dim travel = CSng(p * (len + trail + headLen)) - headLen 'enters and leaves fully off-screen
            Dim head = If(reverse, len - travel - headLen, travel)
            Dim trailStart = If(reverse, head + headLen, head - trail)
            Lit(g, Part(line, horiz, trailStart, trail), horiz, If(reverse, Neon, Rail), If(reverse, Rail, Neon), True)
            Lit(g, Part(line, horiz, head, headLen), horiz, Core, Core, True)
        End Sub

        'Style 3: TRON light cycle. The bike drives ahead and its neon wall builds up behind it;
        'at the far end the wall derezzes (fades) and the bike drives back the other way.
        Private Sub DrawLightCycle(g As Graphics, line As RectangleF, len As Integer, t As Double)
            Const Drive As Double = 4.0, Derez As Double = 0.8
            Dim bikeLen = BikeArea * 3.0F
            Dim cyc = t Mod (2 * (Drive + Derez))
            Dim goingLeft = cyc >= Drive + Derez
            Dim s = cyc Mod (Drive + Derez)

            Lit(g, line, True, Rail, Rail, False)
            If s >= Drive Then 'derez: the whole wall fades back to the rail
                Dim c = Mix(Rail, Neon, 1.0 - (s - Drive) / Derez)
                Lit(g, line, True, c, c, True)
                Return
            End If

            Dim front = CSng(s / Drive * (len + bikeLen)) 'distance of the bike's nose from the start edge
            Dim tail = Math.Max(0.0F, front - bikeLen)
            If tail > 0 Then
                Dim hot = Math.Min(tail, 80.0F)
                If goingLeft Then
                    Lit(g, Part(line, True, len - tail + hot, tail - hot), True, Neon, Neon, True)
                    Lit(g, Part(line, True, len - tail, hot), True, Core, Neon, True)
                Else
                    Lit(g, Part(line, True, 0, tail - hot), True, Neon, Neon, True)
                    Lit(g, Part(line, True, tail - hot, hot), True, Neon, Core, True)
                End If
            End If
            Dim bikeY = If(OuterUp, Lane - BikeArea, Lane + Thickness)
            DrawBike(g, If(goingLeft, len - front, front - bikeLen), bikeY, goingLeft)
        End Sub

        'Bike silhouette in a 48 x 16 box facing right, scaled to the strip and mirrored when going left
        Private Sub DrawBike(g As Graphics, x As Single, y As Integer, faceLeft As Boolean)
            Dim sc = BikeArea / 16.0F
            Dim st = g.Save()
            g.TranslateTransform(If(faceLeft, x + 48 * sc, x), y)
            g.ScaleTransform(If(faceLeft, -sc, sc), sc)
            Dim hull() As PointF = {New PointF(3, 10), New PointF(12, 4), New PointF(24, 2), New PointF(36, 4), New PointF(46, 9), New PointF(44, 13), New PointF(5, 13)}
            If GlowPx > 0 Then 'bloom around the outline
                For i = GlowPx To 1 Step -1
                    Dim k = 1.0 - i / (GlowPx + 1.0)
                    Using p As New Pen(Color.FromArgb(Math.Min(255, CInt(200.0 * k * k / GlowPx) + 3), Neon), 1.5F + 2.0F * i / sc)
                        p.LineJoin = Drawing2D.LineJoin.Round
                        g.DrawPolygon(p, hull)
                    End Using
                Next
            End If
            Using b As New SolidBrush(Color.FromArgb(10, 15, 20)), p As New Pen(Neon, 1.5F), w As New Pen(Neon, 2.0F), hot As New Pen(Core, 1.2F)
                g.FillPolygon(b, hull)
                g.DrawPolygon(p, hull)
                g.DrawLine(hot, 14, 6.5F, 38, 7.5F)                                 'light stripe along the body
                g.FillEllipse(b, 3, 6, 10, 10) : g.DrawEllipse(w, 3, 6, 10, 10)     'rear wheel
                g.FillEllipse(b, 35, 6, 10, 10) : g.DrawEllipse(w, 35, 6, 10, 10)   'front wheel
            End Using
            g.Restore(st)
        End Sub

        '---------- push the bitmap to the layered window ----------
        <StructLayout(LayoutKind.Sequential)> Private Structure SIZEL
            Public cx As Integer, cy As Integer
        End Structure
        <StructLayout(LayoutKind.Sequential)> Private Structure POINTL
            Public x As Integer, y As Integer
        End Structure
        <StructLayout(LayoutKind.Sequential, Pack:=1)> Private Structure BLENDFUNCTION
            Public BlendOp As Byte, BlendFlags As Byte, SourceConstantAlpha As Byte, AlphaFormat As Byte
        End Structure
        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function UpdateLayeredWindow(hwnd As IntPtr, hdcDst As IntPtr, pptDst As IntPtr, ByRef psize As SIZEL, hdcSrc As IntPtr, ByRef pptSrc As POINTL, crKey As Integer, ByRef pblend As BLENDFUNCTION, dwFlags As Integer) As Boolean
        End Function
        <DllImport("user32.dll")> Private Shared Function GetDC(hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("user32.dll")> Private Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
        End Function
        <DllImport("gdi32.dll")> Private Shared Function CreateCompatibleDC(hdc As IntPtr) As IntPtr
        End Function
        <DllImport("gdi32.dll")> Private Shared Function DeleteDC(hdc As IntPtr) As Boolean
        End Function
        <DllImport("gdi32.dll")> Private Shared Function SelectObject(hdc As IntPtr, obj As IntPtr) As IntPtr
        End Function
        <DllImport("gdi32.dll")> Private Shared Function DeleteObject(obj As IntPtr) As Boolean
        End Function

        Private Sub Present(bmp As Bitmap)
            Dim screenDc = GetDC(IntPtr.Zero)
            Dim memDc = CreateCompatibleDC(screenDc)
            Dim hBmp = bmp.GetHbitmap(Color.FromArgb(0)) 'keeps the alpha channel
            Dim old = SelectObject(memDc, hBmp)
            Try
                Dim size As New SIZEL With {.cx = bmp.Width, .cy = bmp.Height}
                Dim src As New POINTL
                Dim blend As New BLENDFUNCTION With {.BlendOp = 0, .BlendFlags = 0, .SourceConstantAlpha = 255, .AlphaFormat = 1} 'AC_SRC_ALPHA
                UpdateLayeredWindow(Handle, screenDc, IntPtr.Zero, size, memDc, src, 0, blend, 2) 'ULW_ALPHA, keep position
            Finally
                SelectObject(memDc, old)
                DeleteObject(hBmp)
                DeleteDC(memDc)
                ReleaseDC(IntPtr.Zero, screenDc)
            End Try
        End Sub
    End Class
End Class
