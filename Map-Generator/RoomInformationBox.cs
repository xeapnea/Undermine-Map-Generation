using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Map_Generator.Math;
using Map_Generator.Parsing.Json.Classes;
using Map_Generator.Parsing.Json.Enums;

namespace Map_Generator
{
    public sealed class RoomInformationBox : Panel
    {
        private readonly Form _form;
        private readonly Room[] _rooms; // Array of all rooms
        private int _textOffset = 40;
        private int _rowSize = 30;
        private const int GapSize = 5;

        public RoomInformationBox(Form form, Room[] rooms)
        {
            _form = form;
            _rooms = rooms; // Assign all rooms
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Vector2Int position = new Vector2Int(10, 0);

            // Check if a room is selected
            if (_rooms.All(r => r != null) && _rooms.Any(r => r == null))
            {
                // If not, draw all room numbers
                foreach (Room room in _rooms)
                {
                    if (room != null)
                    {
                        Draw(g, null, $"{room.Name}_{room.Encounter?.Name}", position);
                        position.y += _rowSize + GapSize;
                    }
                }
                return;
            }

            // If a room is selected, proceed with drawing selected room's information
            Room selectedRoom = _rooms.FirstOrDefault(r => r != null)!;

            // draw room name
            Draw(g, MapIconExtension.GetMapImage(selectedRoom).FirstOrDefault(), $"{selectedRoom.Name}_{selectedRoom.Encounter?.Name}",
                position);

            // draw door cost
            if (selectedRoom.Encounter is { Door: not Door.None and not Door.Normal })
                Draw(g, selectedRoom.Encounter.Door.GetDoorImage(), $"Door: {selectedRoom.Encounter.Door.ToString()}",
                    position);

            position.y += _rowSize + GapSize;

            // draw room enemies
            if (selectedRoom.Encounter?.RoomEnemies != null)
                foreach (Enemy? enemy in selectedRoom.Encounter.RoomEnemies)
                {
                    Draw(g, enemy.EnemyIcon.GetEnemyImage(), enemy.Name, position);
                    position.y += _rowSize + GapSize;
                }

            position.x += 100;
            position.y = _rowSize + (GapSize * 2);

            // draw room extras
            foreach (Item? item in selectedRoom.Extras)
            {
                Draw(g, item.ItemIcon.GetItemImage(), item.Name, position);
                position.y += _rowSize + GapSize;
            }
        }

        private void Draw(Graphics g, Image? image, string text, Vector2Int position)
        {
            if (image != null && position.y < _form.Height)
            {
                float aspectRatio = (float)image.Width / image.Height;
                g.DrawImage(image, new Rectangle(position.x, position.y, (int)(aspectRatio * _rowSize), _rowSize));
            }

            g.DrawString(text, Font, Brushes.Black, new Point(position.x + _textOffset, position.y));
        }
    }
}
