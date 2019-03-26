using SS14.Shared.Interfaces.Serialization;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Interfaces.Players.Appearance
{
    /// <summary>
    ///     What a character looks like.
    /// </summary>
    /// <remarks>
    ///     This is used both as the Actual/Current/Now representation of a Character's appearance data
    ///     and what gets serialised as part of a Player's preferences.
    ///
    ///     When implementing serialization for your <see cref="ICharacterAppearance"/> implementations. (You must do this.)
    ///     the serialized format (eg: YAML) must contain a 'type' field, which is the name of the C# class you made that implements <see cref="ICharacterAppearance"/>.
    /// </remarks>
    public interface ICharacterAppearance
    {
        /*
         * This is implementation specific:
         * Humanoids have hair (and probably a species)
         * Silicons have skins or beep boop styles.
         * AIs have a core icon.
         */
    }
}
