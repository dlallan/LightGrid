/************************************************************
 * File: Form1.cs                                           *
 * Author: Dillon Allan                                     *
 * Description: Main form file for Lab 01.                  *
 ***********************************************************/
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GDIDrawer;

namespace CMPE2800DAllanLab01
{
    public partial class Form1 : Form
    {
        // members
        Thread _tRendering, _tMouseClicks, _tMouseMovement; // three background threads
        Color[,] _aBlockColors; // array of block colors
        static CDrawer _canvas; // drawer for blocks

        public Form1()
        {
            InitializeComponent();
            
            GetNewCanvas();     // init canvas object
            GetNewColorArray(); // start off with the primary color

            // Initialize threads with their looping methods
            // (clicks and movement threads have arbitrary stack sizes
            // for preventing stack overflow.)
            _tRendering = new Thread(TRendering);
            _tMouseClicks = new Thread(TMouseClicks, 99999999);
            _tMouseMovement = new Thread(TMouseMovement, 99999999); 

            // all threads must run in the background
            _tRendering.IsBackground = true;
            _tMouseClicks.IsBackground = true;
            _tMouseMovement.IsBackground = true;

            // start all threads.
            _tRendering.Start();
            _tMouseClicks.Start();
            _tMouseMovement.Start();
        }
    }
}