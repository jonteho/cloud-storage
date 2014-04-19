<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudPhotoApp.Service" generation="1" functional="0" release="0" Id="a3cbebe5-e408-4eac-8677-e8f133af8e3d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudPhotoApp.ServiceGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="PhotoWorkerRole:PhotoAppStorage" defaultValue="">
          <maps>
            <mapMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/MapPhotoWorkerRole:PhotoAppStorage" />
          </maps>
        </aCS>
        <aCS name="PhotoWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/MapPhotoWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapPhotoWorkerRole:PhotoAppStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/PhotoWorkerRole/PhotoAppStorage" />
          </setting>
        </map>
        <map name="MapPhotoWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/PhotoWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="PhotoWorkerRole" generation="1" functional="0" release="0" software="C:\Users\Jonathan\SkyDrive\Kurser\Cloud Computing\CloudPhotoApp\CloudPhotoApp.Service\csx\Debug\roles\PhotoWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="PhotoAppStorage" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;PhotoWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;PhotoWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/PhotoWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/PhotoWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudPhotoApp.Service/CloudPhotoApp.ServiceGroup/PhotoWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="PhotoWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="PhotoWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="PhotoWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>