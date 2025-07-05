using Microsoft.Xna.Framework;

namespace PingPongMono.Entities;

public class Paddle(int x, int y, int width, int height) 
    : PingPongEntity(new Rectangle(x, y, width, height));