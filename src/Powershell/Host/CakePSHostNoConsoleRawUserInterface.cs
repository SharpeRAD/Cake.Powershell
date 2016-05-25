using System;
using System.Management.Automation.Host;

namespace Cake.Powershell
{
    /// <summary>
    /// An implementation of <see cref="PSHostRawUserInterface"/> that is used when an app has no console for output.
    /// </summary>
    /// <seealso cref="PSHostRawUserInterface" />
    internal sealed class CakePSHostNoConsoleRawUserInterface : PSHostRawUserInterface
    {
        /// <summary>
        /// There is no console, so a ConsoleColor of black is returned.
        /// </summary>
        public override ConsoleColor BackgroundColor
        {
            get
            {
                return ConsoleColor.Black;
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so buffer size of 300x5000 is returned to prevent crashes.
        /// </summary>
        public override Size BufferSize
        {
            get
            {
                return new Size(300, 5000);
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so an empty Coordinates instance is returned.
        /// </summary>
        public override Coordinates CursorPosition
        {
            get
            {
                return new Coordinates();
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so a cursor size of 1 is returned.
        /// </summary>
        public override int CursorSize
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so a ConsoleColor of white is returned.
        /// </summary>
        public override ConsoleColor ForegroundColor
        {
            get
            {
                return ConsoleColor.White;
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no color, so false is returned to indicate no key was pressed.
        /// </summary>
        public override bool KeyAvailable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// There is no console, so the value of <see cref="BufferSize"/> is returned.
        /// </summary>
        public override Size MaxPhysicalWindowSize
        {
            get
            {
                return BufferSize;
            }
        }


        /// <summary>
        /// There is no console, so the value of <see cref="BufferSize"/> is returned.
        /// </summary>
        public override Size MaxWindowSize
        {
            get
            {
                return BufferSize;
            }
        }

        /// <summary>
        /// There is no console, so empty Coordinates are returned.
        /// </summary>
        public override Coordinates WindowPosition
        {
            get
            {
                return new Coordinates();
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so the value of <see cref="BufferSize"/> is returned.
        /// </summary>
        public override Size WindowSize
        {
            get
            {
                return BufferSize;
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so an empty string is returned.
        /// </summary>
        public override string WindowTitle
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        /// <summary>
        /// There is no console, so this is not implemented.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// There is no console, so this is not implemented.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// There is no console, does nothing
        /// </summary>
        public override void FlushInputBuffer()
        {
        }

        /// <summary>
        /// There is no console, does nothing
        /// </summary>
        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
        }

        /// <summary>
        /// There is no console, does nothing
        /// </summary>
        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
        }

        /// <summary>
        /// There is no console, does nothing
        /// </summary>
        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
        }
    }
}