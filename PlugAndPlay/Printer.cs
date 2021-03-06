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
using System.Diagnostics;
using System.IO;
using CSPlang;

namespace PlugAndPlay
{

    /**
     * Prints each <TT>Object</TT> from its input channel to a <TT>PrintStream</TT>.
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files/Printer1.gif"></p>
     * <H2>Description</H2>
     * <TT>Printer</TT> is a process for printing each object from its
     * <TT>in</TT> channel to a <TT>PrintStream</TT> object (by default,
     * <TT>System.out</TT>).
     * <P>
     * For convenience, <TT>Printer</TT> may be configured with prefix and postfix
     * strings with which to decorate its output.
     *
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Input Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>in</TH>
     *     <TD>java.lang.Object</TD>
     *     <TD>
     *       The in Channel can accept data of any Class.
     *     </TD>
     *   </TR>
     * </TABLE>
     *
     * <H2>Example</H2>
     * See the example in {@link Merge}.
     *
     * @see Parallel
     *
     * @author P.H. Welch and P.D. Austin
     */

    public class Printer : IamCSProcess
    {
        /** The channel from which to read */
        private ChannelInput In;

        /** The stream to which to write */
        private StreamWriter printStream;

        /** The string to write in front of each object */
        private String prefix;

        /** The string to write after each object */
        private String postfix;

        /**
         * Construct a new <TT>Printer</TT> with <TT>System.out</TT> as
         * its <TT>PrintStream</TT> and empty prefix and postfix strings.
         *
         * @param in the channel from which to read
         */
        public Printer(ChannelInput In) : this(In, new StreamWriter(Console.OpenStandardOutput()), "", "")
        {

        }

        /**
         * Construct a new <TT>Printer</TT> with <TT>System.out</TT> as
         * its <TT>PrintStream</TT>.
         *
         * @param in the channel from which to read
         * @param prefix the string to write in front of each object
         * @param postfix the string to write after each object
         */
        public Printer(ChannelInput In, String prefix, String postfix) : this(In, new StreamWriter(Console.OpenStandardOutput()), prefix, postfix)
        {

        }

        /**
         * Construct a new <TT>Printer</TT> with empty prefix and postfix strings.
         *
         * @param in the channel from which to read
         * @param printStream the stream to which to write
         */
        public Printer(ChannelInput In, StreamWriter printStream) : this(In, printStream, "", "")
        {

        }

        /**
         * Construct a new <TT>Printer</TT>.
         *
         * @param in he channel from which to read
         * @param printStream the stream to which to write
         * @param prefix the string to write in front of each object
         * @param postfix the string to write after each object
         */
        public Printer(ChannelInput In, StreamWriter printStream, String prefix, String postfix)
        {
            this.In = In;
            this.printStream = printStream;
            this.prefix = prefix;
            this.postfix = postfix;
        }

        /**
         * The main body of this process.
         */
        public /*synchronized*/ void run()
        {
            try
            {
                while (true)
                {
                    printStream.Write(prefix);
                    printStream.Write(In.read());
                    printStream.Write(postfix);
                    printStream.Flush();
                }
            }
            catch (PoisonException p)
            {
                // nothing to do
            }
        }
    }
}