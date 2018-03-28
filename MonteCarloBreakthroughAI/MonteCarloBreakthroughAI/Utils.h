#pragma once
enum PlayerColor
{
    White = 0,
    Black = 1,
    Neither = 2,
};
enum Direction 
{
    East = 0,
    Forward = 1,
    West = 2,
    BAD = 3,
};

static PlayerColor FlipColor(PlayerColor color)
{
    if (color == PlayerColor::Black)
    {
        return PlayerColor::White;
    }

    return PlayerColor::Black;
}

class Move
{
public:
    int xCoordinate;
    int yCoordinate;
    Direction direction;
};