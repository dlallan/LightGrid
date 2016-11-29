/************************************************************
 * File: Utils.cs                                           *
 * Author: Dillon Allan                                     *
 * Description: Custom methods and classes used in Lab 01.  *
 ***********************************************************/
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GDIDrawer;

namespace CMPE2800DAllanLab01
{
    // custom functions and properties for main form
    public partial class Form1 : Form
    {
        // different block states used for recursion and highlighting.
        public enum BlockState
        {
            visited,    // highlight me!
            unvisited   // don't highlight me, please...
        }

        // properties used by main form class
        public struct Block
        {
            public delegate void delVoidPoint(Point pCoords);   // block position delegate
            public delegate void delVoidInt(int iCount);        // TEST delegate for updating block count
            public static bool _bIsDirty = true;                // dirty flag for rendering the canvas
            public static object _oLock = new object();         // object for locks
            public const int _iWidth = 800;                     // canvas pixel width
            public const int _iHeight = 800;                    // canvas pixel height
            public const int _iBlockSize = 50;                  // for performance, block size must be no less than 10.
            public static Color _cPrimary = Color.FromArgb(127, 245, 241, 222);     // beige
            public static Color _cAlternate = Color.FromArgb(127, 255, 20, 147);    // deep pink

            // array size -> scaled canvas dimensions.
            public static BlockState[,] _aBlockProgress = new BlockState[_iWidth / _iBlockSize,
                                                                            _iHeight / _iBlockSize];
        }

        // Using const values from Block class, initialize _canvas and set its scale.
        // (only used once, so no locks are necessary.)
        private void GetNewCanvas()
        {
            _canvas = new CDrawer(Block._iWidth, Block._iHeight, bContinuousUpdate: false); // manual render
            _canvas.Scale = Block._iBlockSize;
        }

        // Use canvas properties to initialize _aBlockColors, and set each element to the primary color.
        // (only used once, so no locks are necessary.)
        private void GetNewColorArray()
        {    
            _aBlockColors = new Color[_canvas.ScaledWidth, _canvas.ScaledHeight];
            for (int x = 0; x < _aBlockColors.GetLength(0); x++)
                for (int y = 0; y < _aBlockColors.GetLength(1); y++)
                    _aBlockColors[x, y] = Block._cPrimary;
            ResetState(); // set all blocks with no highlighting initially.
        }

        // set all elements of _aBlockProgress as unvisited.
        private void ResetState()
        {
            // take a snapshot of the progress array.
            BlockState[,] progressCopy;
            lock(Block._oLock) 
                progressCopy = (BlockState[,])Block._aBlockProgress.Clone();
            
            for (int i = 0; i < progressCopy.GetLength(0); i++)
                for (int j = 0; j < progressCopy.GetLength(1); j++)
                    progressCopy[i, j] = BlockState.unvisited;
            
            // reset main progress array
            lock (Block._oLock)
                Block._aBlockProgress = (BlockState[,])progressCopy.Clone();
        }

        // add blocks pixel-by-pixel with highlighting based 
        // on their corresponding _aBlockState value.
        private void RenderBlocks()
        {
            lock (Block._oLock)
                _canvas.Clear();
            
            // take a snapshot of the progress array.
            BlockState[,] progressCopy;
            lock (Block._oLock)
                progressCopy = (BlockState[,])Block._aBlockProgress.Clone();

            // do the same with the color array
            Color[,] colorCopy;
            lock (Block._oLock)
                colorCopy = (Color[,])_aBlockColors.Clone();

            // iterate over canvas, pixel by pixel.
            for (int x = 0; x < _canvas.ScaledWidth; x++)   
            {
                for (int y = 0; y < _canvas.ScaledHeight; y++)
                {
                    lock (Block._oLock) // add block with color highlighting based on its state
                        _canvas.AddRectangle(x, y, 1, 1, 
                            progressCopy[x, y] == BlockState.unvisited ? 
                            colorCopy[x,y] : Color.FromArgb(255, _aBlockColors[x, y]), 
                            _canvas.Scale / 10, Color.Black);
                }
            }
            
            lock (Block._oLock)
                _canvas.Render();
            Thread.Sleep(30); // give the canvas time to render large collections of 
                              // blocks before clearing 
                              // (causing canvas "flickering" as a result for small block sizes.)
        }

        // main method for _tRendering thread.
        // Renders _canvas only if dirty flag is set.
        private void TRendering()
        {
            while (true)
            {
                lock (Block._oLock)
                    if (Block._bIsDirty) // only render if dirty flag is set
                    {
                        RenderBlocks();
                        Block._bIsDirty = false; // reset dirty flag!
                    }
                Thread.Sleep(1);
            }
        }

        // main method for _tMouseClicks thread.
        // Polls for mouse clicks on _canvas, and sets color of block.
        // Left click - alternate color
        // Right click - primary color
        private void TMouseClicks()
        {
            Point pMousePos; // point used for mouse position
            while (true)
            { 
                if (_canvas.GetLastMouseLeftClickScaled(out pMousePos)) // poll left mouse clicks
                {
                    // take snapshot of main color array
                    Color[,] colorCopy;
                    lock (Block._oLock)
                        colorCopy = (Color[,])_aBlockColors.Clone();

                    colorCopy[pMousePos.X, pMousePos.Y] = Block._cAlternate; // set block color to alternate

                    // set main color array to snapshot clone
                    lock (Block._oLock) 
                        _aBlockColors = (Color[,])colorCopy.Clone(); 
                    
                    GetNewCount(pMousePos.X, pMousePos.Y); // get new block count
                }
                if (_canvas.GetLastMouseRightClickScaled(out pMousePos)) // poll right mouse clicks
                {
                    // take snapshot of main color array
                    Color[,] colorCopy;
                    lock (Block._oLock)
                        colorCopy = (Color[,])_aBlockColors.Clone();

                    colorCopy[pMousePos.X, pMousePos.Y] = Block._cPrimary; // set block color to primary

                    // set main color array to snapshot clone
                    lock (_aBlockColors)
                        _aBlockColors = (Color[,])colorCopy.Clone();

                    GetNewCount(pMousePos.X, pMousePos.Y); // get new block count
                }
                Thread.Sleep(1);
            }
        }

        // main method for _tMouseMovement thread.
        // polls for new mouse position. Updates UI with new block coords, and window coords.
        // Checks for adjacent blocks of the same color as block under mouse coords,
        // and updates the UI with a counter value.
        private void TMouseMovement()
        {
            Point pMousePos;
            while (true)
            {
                if (_canvas.GetLastMousePositionScaled(out pMousePos)) //poll for new mouse scaled position.
                {
                    try // try to update the block position UI
                    {
                        Invoke(new Block.delVoidPoint(CBUpdateBlockPosition), pMousePos); //update block coords label.
                    }
                    catch (System.Reflection.TargetException e)
                    {
                        System.Diagnostics.Trace.WriteLine($"Error occured at Block Position Update invoke: \n{e.StackTrace}");
                    }
                    GetNewCount(pMousePos.X, pMousePos.Y); // get new block count
                }
                if (_canvas.GetLastMousePosition(out pMousePos)) // poll for new mouse pixel position
                {
                    try // try to update the block position UI
                    {
                        Invoke(new Block.delVoidPoint(CBUpdateWinPosition), pMousePos);
                    }
                    catch (System.Reflection.TargetException e)
                    {
                        System.Diagnostics.Trace.WriteLine($"Error occured at Window Position Update invoke: \n{e.StackTrace}");
                    }
                }
                Thread.Sleep(1);
            }
        }
        
        // Makes copies of the color and state members, so that 
        // the recursive fct GetAdjacentBlocks can use reference vars
        // without causing crossthreading issues.
        private void GetNewCount(int x, int y)
        {
            Color[,] colorCopy;
            BlockState[,] stateCopy;
            int iCount; // container for recursive "roll-up" sum 

            ResetState(); // reset all blocks to unvisited

            // take snapshots of main color and state arrays
            lock (Block._oLock)
            {
                colorCopy = (Color[,])_aBlockColors.Clone();
                stateCopy = (BlockState[,])Block._aBlockProgress.Clone();
            }
            
            //check for adjacent blocks having the same color, and get the new block count
            iCount = GetAdjacentBlocks(x, y, colorCopy[x, y], ref colorCopy, ref stateCopy);

            try // try to update the block count UI
            {
                Invoke(new Block.delVoidInt(CBUpdateBlockCount), iCount);
            }
            catch (System.Reflection.TargetException e)
            {
                System.Diagnostics.Trace.WriteLine($"Error occured at Block Count Update invoke: \n{e.StackTrace}");
            }

            //update class members
            lock (Block._oLock)
            {
                _aBlockColors = (Color[,])colorCopy.Clone();
                Block._aBlockProgress = (BlockState[,])stateCopy.Clone();
                Block._bIsDirty = true; //trigger new render for new highlighting
            }   
        }        

        // Recursive fct WITH roll-up, as per our discussion on 2016-09-19, Simon :)
        // Requires two reference variables to track and update the color and state coords.
        // This seems to be safe, as long as the ref vars are never accessed elsewhere during execution,
        // which is ensured in my GetNewCount method above, as they are just local vars instead of the class members
        // I used previously...*phew*
        private int GetAdjacentBlocks(int x, int y, Color color, ref Color[,] colorMap, ref BlockState[,] stateMap)
        {
            // return if out of bounds
            if (x < 0 || x > colorMap.GetLength(0) - 1 ||
                y < 0 || y > colorMap.GetLength(1) - 1)
                return 0;

            if (stateMap[x, y] == BlockState.visited) return 0; // return if visited already

            if (colorMap[x, y] != color) return 0; // return if different color

            stateMap[x, y] = BlockState.visited; // same color confirmed - mark this block as officially visited
            
            // check adjacent blocks' colors recursively, returning the "roll-up" sum of all adjacent colors!
            return 1 + 
                   GetAdjacentBlocks(x + 1, y, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x + 1, y + 1, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x, y + 1, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x - 1, y + 1, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x - 1, y, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x - 1, y - 1, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x, y - 1, color, ref colorMap, ref stateMap) +
                   GetAdjacentBlocks(x + 1, y - 1, color, ref colorMap, ref stateMap);
        }

        // call back fct for updating the block position UI
        private void CBUpdateBlockPosition(Point pos)
        {
            lab_BlockPosition.Text = $"{{ X= {pos.X}, Y= {pos.Y} }}";
        }

        // call back fct for updating the windows position UI
        private void CBUpdateWinPosition(Point pos)
        {
            lab_WinPosition.Text = $"{{ X= {pos.X}, Y= {pos.Y} }}";
        }
        
        // call back fct for updating the block count UI
        private void CBUpdateBlockCount(int iCount)
        {
            lab_BlockCount.Text = iCount.ToString();
        }
    }
}
