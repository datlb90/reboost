
<template>
  <div id="privacy-policy">
    <h1 style="margin-bottom: 20px;"><a href="/" style="font-weight: 300">Reboost</a> Privacy Policy</h1>
    <p>Effective Date: 15/11/2023</p>
    <p>This Privacy Policy outlines how Reboost collects, uses, and protects your personal information when you use our website https://reboost.vn (the "Site") and the services provided by Reboost (the "Services"). By using our Site and Services, you agree to the terms outlined in this policy.</p>

    <h3>Information We Collect</h3>

    <p><b>1. User Account Information</b></p>
    <p>When you create an account on Reboost, we collect the following information:</p>
    <p>- Full Name</p>
    <p>- Phone Number</p>
    <p>- Email Address</p>
    <p>- Password (encrypted)</p>

    <p><b>2. Usage Data</b></p>
    <p>We automatically collect information about your interactions with our Site and Services, including:</p>
    <p>- Log data (IP address, browser type, pages visited)</p>
    <p>- Device information (device type, operating system)</p>

    <p><b>3. Writing Practice Data</b></p>
    <p>As a platform focused on improving English writing skills, we collect the texts you submit for practice and the feedback provided by our system.</p>

    <h3>How We Use Your Information</h3>
    <p>We use the collected information for the following purposes:</p>
    <p>- To provide and improve our Services</p>
    <p>- To personalize your experience</p>
    <p>-  To communicate with you (e.g., updates, feedback)</p>

    <h3>Data Security</h3>
    <p>We take the security of your personal information seriously. Reboost implements measures to protect customer data including  from unauthorized access, alteration, disclosure, or destruction.</p>
    <p>Our data security policy aims to ensure the confidentiality, integrity, and availability of customer personal information stored in Azure SQL databases by employing encryption and other security measures.</p>
    <p><strong>Data Encryption At Rest: </strong>All customer personal information stored in Azure SQL databases must be encrypted using Azure SQL Database Transparent Data Encryption (TDE) to ensure data at rest is encrypted.</p>
    <p><strong>Data Encryption In Transit: </strong>Use Azure's built-in transport layer security (TLS) to encrypt data in transit between Azure services and clients accessing the database.</p>
    <p><strong>Data Access Control: </strong>Implement Azure Active Directory (Azure AD) for identity and access management. Ensure strict access control policies are in place, granting access based on the principle of least privilege. Besides, we also use Azure SQL Database's built-in firewall and network security features to restrict access to the database.</p>

    <h3>Sharing Your Information</h3>
    <p>We do not sell, trade, or otherwise transfer your personal information to third parties without your consent, except as outlined in this Privacy Policy or as required by law.
    </p>

    <h3>Your Choices</h3>
    <p>You can manage your account preferences and opt-out of certain communications. However, some essential communications related to the Services may be necessary.
    </p>

    <h3>Cookies</h3>
    <p> Reboost may use cookies to enhance your experience. You can manage your cookie preferences through your browser settings.
    </p>

    <h3>Changes to Privacy Policy</h3>
    <p>Reboost reserves the right to update this Privacy Policy. We will notify you of any changes by posting the new policy on this page.
    </p>

    <h3> Contact Us</h3>
    <p>If you have any questions or concerns about this Privacy Policy, please contact us at <a href="mailto:support@reboost.vn">support@reboost.vn</a></p>

  </div>
</template>
<script>
export default {
  name: 'Privacy',
  data() {
    return {
    }
  }
}
</script>
<style>
#privacy-policy {
  width: 900px;
  margin: auto;
  padding: 40px;
  background: #edeeef;
}
</style>

