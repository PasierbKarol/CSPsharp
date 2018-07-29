using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    class Spurious
    {
        /**
   * If logging is required, this flag should be set <i>before</i> any concurrency
   * is started.  It should only be set <i>once</i> using {@link SpuriousLog#start()}.
   * There is no concurrency protection!
   */
        static public Boolean logging = false;

        /**
         * This is the allowed early timeout (in msecs).  Some JVMs timeout on calls
         * of <tt>wait (timeout)</tt> early - this specifies how early JCSP will tolerate.
         * <p>
         * We need this to distinguish between a <i>JVM-early</i> timeout (that should
         * be accepted) and a <i>spurious wakeup</i> (that should not).  The value to
         * which this field should be set is machine dependant.  For JVMs that do not
         * return early timeouts, it should be set to zero.  For many, it should be
         * left at the default value (4).  If {@link Spurious#logging} is enabled,
         * counts of spurious wakeups versus accepted early timeouts on <tt>select</tt>
         * operations on {@link Alternative}s can be obtained; this field should be
         * set to minimise the former.
         * <p>
         * This field should be set <i>before</i> any concurrency is started.
         * It should only be set <i>once</i> using {@link SpuriousLog#setEarlyTimeout(long)}.
         * There is no concurrency protection!
         */
        static public long earlyTimeout = 9;

    }
}
