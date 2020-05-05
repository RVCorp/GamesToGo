using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla que no tiene funcionalidad, y solo demuestra el flujo de pantallas que debe seguir el programa.
    /// </summary>
    public class EmptyScreen : Screen
    {
        //Contenedor de reflujo automatico, contiene los botones para acceder a las siguientes pantallas a esta.
        private readonly FillFlowContainer<BasicButton> nextScreensContainer;
        //Texto de muestra en la pantalla (Muestra nombre de pantalla).
        private readonly SpriteText screenText;
        //Boton para regresar a la pantalla anterior.
        private readonly BasicButton backButton;

        protected virtual bool ShouldShowExit => true;

        //Accion con la cual se puede cerrar la ventana por completo, en caso de necesitarse.
        private Action gameExitAction;

        /// <summary>
        /// TIpos de las pantallas a las que se puede acceder desde esta pantalla.
        /// </summary>
        protected virtual IEnumerable<Type> FollowingScreens => null;

        public EmptyScreen()
        {
            if (ShouldShowExit)
            {
                AddInternal(backButton = new BasicButton
                {
                    AutoSizeAxes = Axes.X,          //Botones del tamaño justo para el texto que tengan...
                    Height = 50,                    //Con altura de 50 pixeles
                    Anchor = Anchor.BottomLeft,     //En la esquina inferior izquierda
                    Origin = Anchor.BottomLeft,
                });
            }
            //Cargamos los componentes que deben estar dentro de la pantalla
            AddRangeInternal(new Drawable[]
            {
                screenText = new SpriteText             //Un sprite de texto...
                {
                    Text = GetType().Name,              //Con el nombre de la clase como texto
                    RelativePositionAxes = Axes.Both,   //Con posicion relativa a la pantalla
                    Anchor = Anchor.Centre,             //Con posición (0, 0) en el centro
                    Origin = Anchor.Centre,
                    X = 1f / 16,                        //Ubicado un 16vo de pantalla a la derecha
                },
                nextScreensContainer = new FillFlowContainer<BasicButton> //Un contenedor de reflujo que contiene botones...
                {
                    Direction = FillDirection.Vertical, //Que acomoda en vertical    
                    Anchor = Anchor.BottomRight, //Ubicado en la esquina inferior derecha
                    Origin = Anchor.BottomRight,
                    AutoSizeAxes = Axes.Both, //Tamaños determinados por su contenido
                }
            });

            //Por defecto, no hay pantallas a las que acceder
            if (FollowingScreens != null)
            {
                //Si una pantalla heredada decide incluir, agregamos un boton por cada proxima pantalla
                foreach (var screen in FollowingScreens)
                {
                    //En lo general, mismas propiedades que el boton de retorno.
                    nextScreensContainer.Add(new BasicButton
                    {
                        AutoSizeAxes = Axes.X,
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Height = 50,
                        Text = $"{screen.Name}",
                        BackgroundColour = getColorFor(screen.Name), //El color es generado a traves del hash del nombre
                        HoverColour = getColorFor(screen.Name).Lighten(0.2f), //Mismo color, pero mas claro.
                        Action = delegate { this.Push(Activator.CreateInstance(screen) as Screen); } //Para la acción creamos una pantalla del tipo indicado y lo agregamos a la pila.
                    });
                }
            }
        }

        //Usualmente, la función load es para acceder a las dependencias ya cargadas
        [BackgroundDependencyLoader]
        private void load(GamesToGoEditor program) //En este caso accedemos a la ventana del juego
        {
            gameExitAction = program.Exit; //Y obtenemos su acción para ser cerrada.
        }

        //Llamada cuando se sale de la pantalla con this.Exit()
        public override bool OnExiting(IScreen next)
        {
            screenText.MoveToX(1f / 16, 1000, Easing.OutExpo);
            this.FadeOut(1000, Easing.OutExpo);

            return base.OnExiting(next);
        }

        //Llamada cuando se hace Push a otra pantalla desde esta
        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            screenText.MoveToX(-1f / 16, 1000, Easing.OutExpo);
            this.FadeOut(1000, Easing.OutExpo);
        }

        //Llamada cuando se regresa a esta pantalla desde otra pantalla a la que se accedió desde esta pantalla.
        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            screenText.MoveToX(0f, 1000, Easing.OutExpo);
            this.FadeIn(1000, Easing.OutExpo);
        }

        //Se llama cuando se agrega esta pantalla a la pila de pantallas
        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            screenText.MoveToX(0f, 1000, Easing.OutExpo);
            this.FadeInFromZero(1000, Easing.OutExpo);

            if (ShouldShowExit)
            {
                //Le damos texto al boton de atrás, nombre de la pantalla anterior en caso de existir.
                backButton.Text = last?.GetType().Name ?? "Exit";

                //Mismo metodo para agregar color a los botones que a los botones de pantallas posteriores.
                backButton.BackgroundColour = last == null ? Color4.IndianRed : getColorFor(last.GetType().Name);
                backButton.HoverColour = backButton.BackgroundColour.Lighten(0.2f);

                //Para la acción, usamos la salida de nuestra pantalla si es posible, si no es posible, la salida de la ventana.
                backButton.Action = last == null ? gameExitAction : this.Exit;
            }
        }

        //Manera de obtener un color diferente para cada nobre de pantalla.
        private static Color4 getColorFor(object type)
        {
            int hash = type.GetHashCode();
            byte r = (byte)Math.Clamp(((hash & 0xFF0000) >> 16) * 0.8f, 20, 255);
            byte g = (byte)Math.Clamp(((hash & 0x00FF00) >> 8) * 0.8f, 20, 255);
            byte b = (byte)Math.Clamp((hash & 0x0000FF) * 0.8f, 20, 255);
            return new Color4(r, g, b, 255);
        }
    }
}
