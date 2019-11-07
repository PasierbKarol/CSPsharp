//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
//  You may use this work under the terms of either                 //
//  1. The Apache License, Version 2.0                              //
//  2. or (at your option), the GNU Lesser General Public License,  //
//       version 2.1 or greater.                                    //
//                                                                  //
//  Full licence texts are included in the LICENCE file with        //
//  this library.                                                   //
//                                                                  //
//  Author contacts: P.H.Welch@kent.ac.uk K.Chalmers@napier.ac.uk   //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using CSPlang;

namespace PlugAndPlay
{
    /**
     * A free-standing array of button processes in their own frame,
     * with <i>configure</i> and <i>channelEvent</i> channels.
     * <H2>Process Diagram</H2>
     * Please check out the process diagram for a framed <i>single</i> button
     * in {@link FramedButton}.
     * Imagine here an array of these, each with individual <i>configure</i> and
     * <i>channelEvent</i> channels.
     * <H2>Description</H2>
     * This process provides a free-standing array of button processes in their
     * own frame.
     * They are just {@link ActiveButton}s wrapped in
     * an {@link ActiveClosingFrame},
     * but save us the trouble of constructing them.
     * They may be displayed in a row or column.
     * <p>
     * Wire them to application processes with <i>configure</i>
     * channels (for setting labels, enabling/disabling and all other
     * configuration options) and <i>channelEvent</i> channels (on which the
     * current label on any button is sent when that button is clicked).
     * Note that all the <i>channelEvents</i> may be streamed to the <i>same</i>
     * channel, provided an <tt>Any2*Channel</tt> is used (as in the example
     * below).
     * </p>
     * <p>
     * Initially, all button labels are <i>empty</i> <tt>java.lang.String</tt>s.
     * To set a button label, send a <tt>java.lang.String</tt> down the appropriate
     * <i>configure</i> channel.
     * </p>
     * <p>
     * Initially, all buttons are <i>enabled</i>.
     * To <i>disable</i> a button, send <tt>java.lang.Boolean.FALSE</tt>
     * down the appropriate <i>configure</i> channel.
     * To <i>enable</i>, send <tt>java.lang.Boolean.TRUE</tt>.
     * </p>
     * <p>
     * For other configuration options, send objects implementing
     * the {@link ActiveButton.Configure} interface.
     * </p>
     * <p>
     * <I>IMPORTANT: it is essential that channelEvent channels from this process are
     * always serviced -- otherwise the Java channelEvent Thread will be blocked and the GUI
     * will stop responding.  A simple way to guarantee this is to use channels
     * configured with overwriting buffers.
     * For example:</I>
     * <PRE>
     *   final One2OneChannel myButtonchannelEvent =
     *     Channel.one2one (new OverWriteOldestBuffer (n));
     * </PRE>
     * <I>This will ensure that the Java channelEvent Thread will never be blocked.
     * Slow or inattentive readers may miss rapidly generated channelEvents, but 
     * the </I><TT>n</TT><I> most recent channelEvents will always be available.</I>
     * </p>
     * <H2>Example</H2>
     * This runs a framed button array in parallel with a simple application process
     * (<i>in-lined</i> in the {@link Parallel <tt>Parallel</tt>} below).
     * All <i>channelEvent</i> channels from the buttons are mulitplexed through
     * an {@link Any2OneChannel} to the application process.
     * The application configures the buttons with their labels, then reports
     * each time any of them is pressed.
     * The application ends when the button labelled <i>`Goodbye World'</i> is pressed.
     * <PRE>
     * import jcsp.lang.*;
     * import jcsp.util.*;
     * import jcsp.plugNplay.*;
     * 
     * public class FramedButtonArrayExample {
     * 
     *   public static void main (String argv[]) {
     * 
     *     // labels for the array of buttons
     * 
     *     final String[] label = {"JCSP", "Rocket Science", "occam-pi", "Goodbye World"};
     * 
     *     final int nButtons = label.Length;
     * 
     *     // row or column?
     * 
     *     final boolean horizontal = true;
     *   
     *     // initial pixel sizes for the frame for the button array
     *     
     *     final int pixDown = 20 + (horizontal ? 120 : nButtons*120);
     *     final int pixAcross = horizontal ? nButtons*120 : 120;
     *   
     *     // all button channelEvents are wired (for this example) to the same channel ...
     * 
     *     final Any2OneChannel allchannelEvents =
     *       Channel.any2one (new OverWriteOldestBuffer (10));
     * 
     *     final Any2OneChannel[] channelEvent = new Any2OneChannel[nButtons];
     *     
     *     for (int i = 0; i < nButtons; i++) {
     *       channelEvent[i] = allchannelEvents;
     *     }
     * 
     *     // each button is given its own configuration channel ...
     * 
     *     final One2OneChannel[] configure = Channel.one2oneArray (nButtons);
     * 
     *     // make the array of buttons ...
     * 
     *     final FramedButtonArray buttons =
     *       new FramedButtonArray (
     *         "FramedButtonArray Demo", nButtons,
     *         pixDown, pixAcross, horizontal,
     *         Channel.getInputArray (configure), Channel.getOutputArray (channelEvent)
     *       );
     * 
     *     // testrig ...
     * 
     *     new Parallel (
     *     
     *       new CSProcess[] {
     *       
     *         buttons,
     *         
     *         new CSProcess () {
     *         
     *           public void run () {
     *     
     *             for (int i = 0; i < nButtons; i++) {
     *               configure[i].out ().write (label[i]);
     *             }
     *             
     *             boolean running = true;
     *             while (running) {
     *               final String s = (String) allchannelEvents.in ().read ();
     *               System.out.println ("Button `" + s + "' pressed ...");
     *               running = (s != label[nButtons - 1]);
     *             }
     *             
     *             System.exit (0);
     *             
     *           }
     *           
     *         }
     *         
     *       }
     *     ).run ();
     * 
     *   }
     * 
     * }
     * </PRE>
     *
     * @see ActiveButton
     * @see FramedButton
     * @see FramedButtonGrid
     * @see FramedScrollbar
     *
     * @author P.H. Welch
     *
     */

    public sealed class FramedButtonArray : IamCSProcess
    {

        /** The frame for the buttons */
        //private readonly ActiveClosingFrame activeClosingFrame;

        /** The buttons */
        //private readonly ActiveButton[] button;

        /**
         * Construct a framed button array process.
         * <p>
         *
         * @param title the title for the frame (must not be null)
         * @param nButtons the number of buttons (must be at least 1)
         * @param pixDown the pixel hieght of the frame (must be at least 100)
         * @param pixAcross the pixel width of the frame (must be at least 100)
         * @param horizontal <tt>true</tt> for a horizontal array of buttons, <tt>false</tt> for a vertical one
         * @param configure the configure channels for the buttons (must not be null)
         * @param channelEvent the channelEvent channels from the buttons (must not be null)
         * 
         */
        public FramedButtonArray(String title, int nButtons,
            int pixDown, int pixAcross, Boolean horizontal,
            ChannelInput[] configure, ChannelOutput[] channelEvent) {

            // check everything ...

            if (title == null)
            {
                throw new ArgumentException(
                    "From FramedButtonArray (title == null)"
                );
            }

            if ((nButtons < 1) || (pixDown < 100) || (pixAcross < 100))
            {
                throw new ArgumentException(
                    "From FramedButtonArray (nButtons < 1) || (pixDown < 100) || (pixAcross < 100)"
                );
            }

            if ((configure == null) || (channelEvent == null)) {
                throw new ArgumentException(
                    "From FramedButtonArray (configure == null)"
                );
            }

            if ((nButtons != configure.Length) || (configure.Length != channelEvent.Length)) {
                throw new ArgumentException(
                    "From FramedButtonArray (nButtons != configure.Length) || (configure.Length != channelEvent.Length)"
                );
            }

            for (int i = 0; i < configure.Length; i++)
            {
                if ((configure[i] == null) || (channelEvent[i] == null)) {
                    throw new ArgumentException(
                        "From FramedButtonArray (configure[i] == null) || (channelEvent[i] == null)"
                    );
                }
            }

            // OK - now build ...

        //    activeClosingFrame = new ActiveClosingFrame(title);
        //    /*final*/ ActiveFrame activeFrame = activeClosingFrame.getActiveFrame();

        //    button = new ActiveButton[nButtons];
        //    for (int i = 0; i < nButtons; i++)
        //    {
        //        button[i] = new ActiveButton(configure[i],  channelEvent[i]);
        //    }

        //    activeFrame.setSize(pixAcross, pixDown);
        //    if (horizontal)
        //    {
        //        activeFrame.setLayout(new GridLayout(1, nButtons));
        //    }
        //    else
        //    {
        //        activeFrame.setLayout(new GridLayout(nButtons, 1));
        //    }

        //    for (int i = 0; i < button.Length; i++)
        //    {
        //        activeFrame.add(button[i]);
        //    }

        //    activeFrame.setVisible(true);

        }

        public void run()
        {
            //new CSPParallel(
            //    new IamCSProcess[]
            //    {
            //        activeClosingFrame,
            //        new CSPParallel(button)
            //    }
            //).run();
        }

    }
}