using Content.Shared.Interfaces.Players.Appearance;
using SS14.Shared.Interfaces.Serialization;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Interfaces.Players.Preferences
{
    /// <summary>
    ///     Stores all the information about a single character.
    ///     It's the recipe for your snowflake!
    /// </summary>
    /// <remarks>
    ///     When implementing serialization for your <see cref="ICharacterProfile"/> implementations. (You must do this.)
    ///     the serialized format (eg: YAML) must contain a 'type' field, which is the name of the C# class you made that implements <see cref="ICharacterProfile"/>.
    /// </remarks>
    ///

    //There are obviously different "types" of profile, eg: Humanoid(CharacterProfile), Cyborg(CharacterProfile)
    //These will be defined in yaml, as a CharacterProfileTypePrototype
    // the yaml just being:
    // - name: "Humanoid"
    //   class: "Humanoid" (matches a field on HumanoidCharacterProfile, which is/has a prototype)

    // - name "Cyborg"
    //   class: "Cyborg"

    //Acruid says "see PrototypeAttribute"/ the [Prototype("X")] stuff, basically XCharacterProfile would be/have an IndexedPrototype (I think)

    public interface ICharacterProfile
    {
        /// <summary>
        ///     What your snowflake is called.
        ///     Also used as the name of the profile.
        /// </summary>
        /// <returns>String name of the profile/character.</returns>
        string Name();

        /// <summary>
        ///     What your snowflake looks like.
        /// </summary>
        /// <returns>
        ///     Returns an implementation of <see cref="ICharacterAppearance"/> matching the implementation of <see cref="ICharacterProfile"/>.
        ///     e.g.
        ///     A HumanoidCharacterProfile would have a matching HumanoidCharacterAppearance.
        ///     A CyborgCharacterProfile would have a matching CyborgCharacterAppearance.
        /// </returns>
        ICharacterAppearance CharacterAppearance();

        #region Serialization

        /// <summary>
        ///     Serialize this <see cref="ICharacterProfile"/> as a <see cref="YamlNode"/>.
        /// </summary>
        /// <returns></returns>
        YamlNode ToYamlNode();

        #endregion
    }
}
