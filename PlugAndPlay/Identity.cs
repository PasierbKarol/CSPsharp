//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
// Copyright 1996-2017 Peter Welch, Paul Austin and Neil Brown      //
//           2005-2017 Kevin Chalmers and Jon Kerridge              //
//                                                                  //
// Licensed under the Apache License, Version 2.0 (the "License");  //
// you may not use this file except in compliance with the License. //
// You may obtain a copy of the License at                          //
//                                                                  //
//      http://www.apache.org/licenses/LICENSE-2.0                  //
//                                                                  //
// Unless required by applicable law or agreed to in writing,       //
// software distributed under the License is distributed on         //
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,  //
// either express or implied. See the License for the specific      //
// language governing permissions and limitations under the License.//
//                                                                  //
//                                                                  //
//                                                                  //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//  Author contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using CSPlang;

namespace PlugAndPlay
{ 
/**
 * This copies its input stream to its output stream, adding a one-place buffer
 * to the stream.
 * <H2>Process Diagram</H2>
 * <p><img src="doc-files\Identity1.gif"></p>
 * <H2>Description</H2>
 * <TT>Identity</TT> is a process stream whose output stream is the same
 * as its input stream.  The difference between a bare wire and a wire
 * into which an <TT>Identity</TT> process has been spliced is that the
 * latter provides a buffering capacity of <I>one more</I> than the bare wires
 * (which is zero for the default semantics of channels).
 * <P>
 * <H2>Channel Protocols</H2>
 * <TABLE BORDER="2">
 *   <TR>
 *     <TH COLSPAN="3">Input Channels</TH>
 *   </TR>
 *   <TR>
 *     <TH>in</TH>
 *     <TD>java.lang.Object</TD>
 *     <TD>
 *       The in Channel can accept data of any class.
 *     </TD>
 *   </TR>
 *   <TR>
 *     <TH COLSPAN="3">Output Channels</TH>
 *   </TR>
 *   <TR>
 *     <TH>out</TH>
 *     <TD>java.lang.Object</TD>
 *     <TD>
 *       The out Channel sends the the same type of data (in
 *       fact, the <I>same</I> data) as is input.
 *     </TD>
 *   </TR>
 * </TABLE>
 *
 * @author P.D.Austin
 */
public sealed class Identity : IamCSProcess
{
   /** The input Channel */
   private ChannelInput In;
   
   /** The output Channel */
   private ChannelOutput Out;
   
   /**
    * Construct a new Identity process with the input Channel in and the
    * output Channel out.
    *
    * @param in the input Channel
    * @param out the output Channel
    */
   public Identity(ChannelInput In, ChannelOutput Out)
   {
       this.In = In;
       this.Out = Out;
        }
   
   /**
    * The main body of this process.
    */
   public void run()
   {
      while (true)
         Out.write(In.read());
   }
}}