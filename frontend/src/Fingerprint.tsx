import { getFingerprint } from '@thumbmarkjs/thumbmarkjs';
import { use, useEffect, useState } from 'react';

export const Fingerprint = () => {
  const [fingerprint, setFingerprint] = useState<string | null>(null);

  useEffect(() => {
    getFingerprint().then((fingerprint) => {
      setFingerprint(fingerprint);
    });
  }, []);

  return (
    <div>
      <p>{fingerprint}</p>
    </div>
  );
};
export default Fingerprint;