﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Radio
{
    /// <summary>
    /// Lógica de interacción para MiniPlayer.xaml
    /// </summary>
    public partial class MiniPlayer : Window
    {
        // Holds the views between song info and controls.
        ControlTemplate miniPlay;
        ControlTemplate miniSong;

        MainWindow mw;

        

        public MiniPlayer()
        {
            InitializeComponent();
            // Saves the views
            miniPlay = (ControlTemplate)FindResource("miniPlay");
            miniSong = (ControlTemplate)this.FindResource("miniSong");
            // Set the view
            miniPlayerMain.Template = miniSong;

            
        }

        /// <summary>
        /// Gets a reference of MainWindow to allow switching from views and for
        /// the bindings.
        /// Note that it also calls the SetBindings function.
        /// </summary>
        /// <param name="MW">Reference to MainWindow</param>
        public void SetOtherView(MainWindow MW)
        {
            mw = MW;
            SetBindings();

            // Initialize and bind the command for Play/Stop
            btnPlayCommand.Command = PlayOrStopCommand.PlayOrStop;
            CommandBinding binding = new CommandBinding();
            binding.Command = PlayOrStopCommand.PlayOrStop;
            binding.Executed += mw.PlayOrStop_Execute;
            binding.CanExecute += mw.PlayOrStop_CanExecute;
            CommandBindings.Add(binding);

            // Command for the Minimize button
            btnChangeView.Command = MinimizeMazimizeCommand.MinimizeOrMaximize;
            CommandBinding bindingView = new CommandBinding();
            bindingView.Command = MinimizeMazimizeCommand.MinimizeOrMaximize;
            bindingView.Executed += mw.MinimizeOrMaximize_Execute;
            bindingView.CanExecute += mw.MinimizeOrMaximize_CanExecute;
            CommandBindings.Add(bindingView);
        }

        /// <summary>
        /// This sets the Bindings from MainWindow to the MiniPlayer.
        /// </summary>
        public void SetBindings()
        {
            // Sets the name
            Binding songBdg = new Binding();
            songBdg.Source = mw.tbSong;
            songBdg.Path = new PropertyPath("Text");
            songName.SetBinding(TextBlock.TextProperty, songBdg);

            // Sets the duration of song
            Binding durationBdg = new Binding();
            durationBdg.Source = mw.tbLastSecond;
            durationBdg.Path = new PropertyPath("Text");
            songDuration.SetBinding(TextBlock.TextProperty, durationBdg);

            // Sets the current second of the play bar
            Binding barBdg = new Binding();
            barBdg.Source = mw.pBar;
            barBdg.Path = new PropertyPath("Value");
            playedBar.SetBinding(ProgressBar.ValueProperty, barBdg);

            // Sets the maximum value of the play bar
            Binding maximumBarBdg = new Binding();
            maximumBarBdg.Source = mw.pBar;
            maximumBarBdg.Path = new PropertyPath("Maximum");
            playedBar.SetBinding(ProgressBar.MaximumProperty, maximumBarBdg);

            // Sets the play/pause state
            Binding playState = new Binding();
            playState.Source = mw.buttonPlay;
            playState.Path = new PropertyPath("Content");
            playOrStop.SetBinding(CheckBox.ContentProperty, playState);

            // Sets the volume
            Binding currVol = new Binding();
            currVol.Source = mw.sldrVolume;
            currVol.Path = new PropertyPath("Value");
            volume.SetBinding(Slider.ValueProperty, currVol);
        }

        /// <summary>
        /// Event to allows drag the mini player window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool wasLeftClick = e.ChangedButton == MouseButton.Left;

            if (wasLeftClick && e.Source is Grid
                || wasLeftClick && e.OriginalSource is Visual)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Changes to the Play/Pause, and Volume controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniPlayerMain_MouseEnter(object sender, MouseEventArgs e)
        {
            miniPlayerMain.Template = miniPlay;
        }

        /// <summary>
        /// Change to the Song info, Played bar, and duration song elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniPlayerMain_MouseLeave(object sender, MouseEventArgs e)
        {
            miniPlayerMain.Template = miniSong;
        }

        /// <summary>
        /// Wrapper to play/stop from this view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            mw.PlayOrStop_Execute(sender, null);
        }

        /// <summary>
        /// Closes the application the small x button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // This only closes the Window
            //this.Close(); 
            Application.Current.Shutdown();
        }
    }
}
